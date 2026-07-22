namespace realtime_auction_platform.EndPoints.Auctions
{
    public sealed record UpdateAuctionRequest
    (
        string Title,
        string Description,
        decimal StartPrice,
        DateTimeOffset StartTime,
        DateTimeOffset EndTime
    );
}
