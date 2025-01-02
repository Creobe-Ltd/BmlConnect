using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace Creobe.BmlConnect;

public sealed class BmlConnectClient
{
    private readonly BmlConnectOptions _options;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serailizerOptions;

    public BmlConnectClient(HttpClient client, IOptions<BmlConnectOptions> options)
    {
        _options = options.Value;
        _httpClient = client;

        _serailizerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
    }

    public async Task<Transaction?> CreateTransactionAsync(CreateTransactionRequest request)
    {
        try
        {
            var normalizedAmount = (int)Math.Round(request.Amount * 100);

            var transaction = new Transaction
            {
                Amount = normalizedAmount,
                Currency = request.Currency,
                CustomerReference = request.CustomerReference,
                LocalId = request.LocalId,
                Provider = request.Provider,
                RedirectUrl = request.RedirectUrl,
                SignMethod = "sha1",
                DeviceId = _options.AppId,
                AppVersion = _options.AppVersion,
                ApiVersion = _options.ApiVersion,
                Signature = GetSignature(normalizedAmount, request.Currency),
            };

            var response = await _httpClient.PostAsJsonAsync("transactions", transaction);
            return await response.Content.ReadFromJsonAsync<Transaction>(_serailizerOptions);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create transaction", ex);
        }
    }

    private string GetSignature(int amount, string currency)
    {
        var signature = $"amount={amount}&currency={currency}&apiKey={_options.ApiKey}";
        
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(signature);
        byte[] hashBytes = SHA1.HashData(inputBytes);

        return Convert.ToHexString(hashBytes).ToLower();
    }
}