# BmlConnect

BmlConnect is a lightweight .NET client for the Bank of Maldives (BML) Connect payment gateway. It wraps the HTTP API behind a strongly typed client, includes helpers for generating request signatures, and ships with a dependency-injection extension so you can drop it into any modern .NET application.

## Features
- Typed client that handles transaction serialization and response parsing.
- Built-in SHA1 signature helper that matches the gateway's requirements.
- `IServiceCollection` extension to configure the client with `HttpClientFactory`.
- Ships as a NuGet package with .NET 9 support.

## Requirements
- .NET 9.0 SDK or later.
- Valid BML Connect credentials (App ID, App key, API version, and app version).

## Installation
Install from NuGet using the CLI:

```shell
dotnet add package Creobe.BmlConnect
```

Or reference it directly in your project file:

```xml
<ItemGroup>
  <PackageReference Include="Creobe.BmlConnect" Version="1.1.0" />
</ItemGroup>
```

## Configuration
The library is configured through `BmlConnectOptions` when registering the client. All properties are required:

| Option | Description |
| --- | --- |
| `ApiVersion` | API version assigned to your application (for example, `v1`). |
| `AppId` | Device/application identifier provided by BML. |
| `ApiKey` | Secret key used for authentication and signature creation. |
| `AppVersion` | Version string you want to advertise to the gateway. |
| `Endpoint` | Base URL for the BML Connect API (for example, `https://example.com/payments/`). |

## Quick Start (Dependency Injection)

```csharp
using System;
using Creobe.BmlConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddBmlConnect(options =>
{
    options.AppId = builder.Configuration["BmlConnect:AppId"]
        ?? throw new InvalidOperationException("Configure BmlConnect:AppId.");
    options.ApiKey = builder.Configuration["BmlConnect:ApiKey"]
        ?? throw new InvalidOperationException("Configure BmlConnect:ApiKey.");
    options.ApiVersion = builder.Configuration["BmlConnect:ApiVersion"] ?? "v1";
    options.AppVersion = builder.Configuration["BmlConnect:AppVersion"] ?? "1.0.0";
    options.Endpoint = builder.Configuration["BmlConnect:Endpoint"]
        ?? "https://api.bmlconnect.example";
});

using var host = builder.Build();
var client = host.Services.GetRequiredService<BmlConnectClient>();

var request = new CreateTransactionRequest(
    Amount: 10.00m,
    Currency: "MVR",
    CustomerReference: "REF-12345",
    LocalId: "LOCAL-67890",
    Provider: "BMLCONNECT",
    RedirectUrl: "https://your-app.example/checkout/success"
);

var transaction = await client.CreateTransactionAsync(request);
```

> **Tip:** When you are already inside an ASP.NET Core app, replace `Host.CreateApplicationBuilder(args)` with `WebApplication.CreateBuilder(args)` and the rest of the setup remains the same.

The client normalizes the amount to laari (cents) before it is sent to the API.

## Creating Signatures
The gateway expects a SHA1 signature based on the amount, currency, and API key. You can generate it yourself or let the client handle it:

```csharp
var signature = client.CreateSha1Signature(10.00m, "MVR");

var request = new CreateTransactionRequest(
    Amount: 10.00m,
    Currency: "MVR",
    CustomerReference: "REF-12345",
    LocalId: "LOCAL-67890",
    Signature: signature
);
```

If you skip the `Signature` property, `CreateTransactionAsync` will generate one using the configured API key. Pass a signature only when you need to reuse it across systems.

## Manually Creating the Client
You can also instantiate the client without DI if you already manage your own `HttpClient`:

```csharp
using Creobe.BmlConnect;
using Microsoft.Extensions.Options;

var options = new BmlConnectOptions
{
    ApiVersion = "v1",
    AppId = "your-app-id",
    ApiKey = "your-api-key",
    AppVersion = "1.0.0",
    Endpoint = "https://api.bmlconnect.example"
};

var httpClient = new HttpClient
{
    BaseAddress = new Uri(options.Endpoint)
};

httpClient.DefaultRequestHeaders.Add("Authorization", options.ApiKey);
httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

var client = new BmlConnectClient(httpClient, Options.Create(options));
```

## Data Contracts
### `CreateTransactionRequest`
- `Amount` (`decimal`): Amount in MVR; internally converted to laari.
- `Currency` (`string`): ISO currency code.
- `CustomerReference` (`string`): Reference visible to the customer.
- `LocalId` (`string`): Unique identifier on your side.
- `SignMethod` (`string?`): Defaults to `sha1`.
- `Signature` (`string?`): Optional if you want to provide your own.
- `Provider` (`string?`): Optional payment provider identifier.
- `RedirectUrl` (`string?`): URL for redirecting the customer after payment.
- `Expiry` (`DateTimeOffset?`): Optional expiration timestamp.

### `Transaction`
Represents the gateway response. Useful fields include `Id`, `ShortUrl`, `Url`, `State`, and `Expires`. See `src/Creobe.BmlConnect/Transaction.cs` for all properties.

## Development
- Restore dependencies and build: `dotnet build --configuration Release`
- The package readme is sourced from this file; keep the root `README.md` up to date before publishing.

## Publishing
Pushing a semantic version tag triggers the GitHub Actions workflow that builds and publishes the package to NuGet.

## License
Distributed under the MIT License. See [`LICENSE`](LICENSE).
