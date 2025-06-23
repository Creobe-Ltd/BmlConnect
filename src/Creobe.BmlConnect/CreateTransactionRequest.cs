namespace Creobe.BmlConnect;

public sealed record CreateTransactionRequest(
    decimal Amount,
    string Currency,
    string CustomerReference,
    string LocalId,
    string? SignMethod = "sha1",
    string? Signature = null,
    string? Provider = null,
    string? RedirectUrl = null);