namespace realtime_auction_platform.EndPoints.Auctions
{
    public sealed record PlaceBidRequest(
    Guid BidderId,
    decimal BidAmount);
}
