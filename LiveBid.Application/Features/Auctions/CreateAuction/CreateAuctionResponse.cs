public sealed record CreateAuctionCommand(
    Guid Id,
    string Title,
    decimal CurrentPrice,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime,
    string Status) ;