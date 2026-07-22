using LiveBid.Domain.Auctions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Application.Common.Interfaces
{
   public interface ILiveBidDbContext
    {

        IQueryable<Auction> AuctionsQuery { get; }
        Task AddAuctionAsync(Auction auction, CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<Auction?> GetAuctionAsync(Guid id, CancellationToken cancellationToken = default);

    
    }
}
 