namespace realtime_auction_platform.EndPoints.Auctions
{
    public sealed class GetAuctionBidRequest
    {
        public int Page { get; init; } = 1;

        public int PageSize { get; init; } = 20;

        public string SortDirection { get; init; } = "desc";


    }
}
