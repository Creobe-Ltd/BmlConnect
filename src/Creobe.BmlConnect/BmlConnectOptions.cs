namespace Creobe.BmlConnect;

public sealed record BmlConnectOptions
{
    public required string ApiVersion { get; init; }
    public required string AppId { get; init; }
    public required string ApiKey { get; init; }
    public required string AppVersion { get; init; }
    public required string Endpoint { get; init; }
}