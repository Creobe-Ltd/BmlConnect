namespace Creobe.BmlConnect;

public sealed record CreateTransactionRequest(
    decimal Amount,
    string Currency,
    string CustomerReference,
    string LocalId,
    string? Provider = null,
    string? RedirectUrl = null);