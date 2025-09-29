namespace Creobe.BmlConnect;

public sealed record Transaction
{
    public DateTimeOffset? Created { get; set; }
    public DateTimeOffset? Expires { get; set; }
    public int? Amount { get; set; }
    public object? QrCode { get; set; }
    public string? SignMethod { get; set; }
    public string? ApiVersion { get; set; }
    public string? AppVersion { get; set; }
    public string? Currency { get; set; }
    public string? CustomerReference { get; set; }
    public string? DeviceId { get; set; }
    public string? Id { get; set; }
    public string? LocalId { get; set; }
    public string? Provider { get; set; }
    public string? RedirectUrl { get; set; }
    public string? ShortUrl { get; set; }
    public string? Signature { get; set; }
    public string? State { get; set; }
    public string? Url { get; set; }
    public string? UrlHash { get; set; }
}