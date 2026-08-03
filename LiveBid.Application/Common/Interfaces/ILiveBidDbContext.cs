using LiveBid.Domain.Auctions;


namespace LiveBid.Application.Common.Interfaces
{
   public interface ILiveBidDbContext
    {
        IQueryable<Auction> AuctionsQuery { get; }
        Task AddAuctionAsync(Auction auction, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<Auction?> GetAuctionAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> UserExistAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Bid?> GetWinningBidAsync(Guid auctionId, CancellationToken cancellationToken = default);
        Task AddBidAsync(Bid bid, CancellationToken cancellationToken = default);
    


    }
}
 