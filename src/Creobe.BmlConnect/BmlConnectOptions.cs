namespace Creobe.BmlConnect;

public sealed record BmlConnectOptions
{
    public required string ApiVersion { get; set; }
    public required string AppId { get; set; }
    public required string ApiKey { get; set; }
    public required string AppVersion { get; set; }
    public required string Endpoint { get; set; }
}