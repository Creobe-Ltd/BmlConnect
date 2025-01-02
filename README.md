# BmlConnect

A simple .NET client for BML payment gateway.

## Installation

Install via NuGet:
```shell
dotnet add package Creobe.BmlConnect
```

## Usage with Dependency Injection

```csharp
// ...existing code...
using Microsoft.Extensions.DependencyInjection;
using Creobe.BmlConnect;

// ...existing code...
var services = new ServiceCollection();

services.AddBmlConnect(options =>
{
    // Configure options here
    options.AppId = "YOUR_APP_ID";
    options.ApiKey = "YOUR_API_KEY";
    options.ApiVersion = "v1";
    options.AppVersion = "1.0.0";
    options.Endpoint = "https://example.com";
});

var serviceProvider = services.BuildServiceProvider();
var bmlClient = serviceProvider.GetRequiredService<BmlConnectClient>();

var transaction = await bmlClient.CreateTransactionAsync(new CreateTransactionRequest(
    10.00m, "MVR", "REF123", "LOCAL123", "demoProvider", "https://your-redirect.url"));
```

## Building

Build the project locally:
```shell
dotnet build --configuration Release
```

## Publishing

When you push a version tag, GitHub Actions workflow will publish the package to NuGet automatically.

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
