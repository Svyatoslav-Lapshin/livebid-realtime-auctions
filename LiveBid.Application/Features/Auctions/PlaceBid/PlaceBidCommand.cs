
namespace LiveBid.Application.Features.Auctions.PlaceBid
{
    public sealed record PlaceBidCommand(Guid AuctionId, Guid BidderId, decimal BidAmount);
    
}
