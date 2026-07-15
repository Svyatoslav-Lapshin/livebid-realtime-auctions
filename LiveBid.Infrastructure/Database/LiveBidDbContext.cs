using LiveBid.Application.Common.Interfaces;
using LiveBid.Domain.Auctions;
using LiveBid.Domain.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Infrastructure.Database
{
    public class LiveBidDbContext:DbContext,ILiveBidDbContext
    {
        public LiveBidDbContext(DbContextOptions<LiveBidDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        public DbSet<Auction> Auctions => Set<Auction>();

        public DbSet<Bid> Bids => Set<Bid>();

        public async Task AddAuctionAsync(Auction auction, CancellationToken cancellationToken = default)
        { 
            await Auctions.AddAsync(auction, cancellationToken);
        }

       public async Task<Auction?> GetAuctionAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await Auctions.FindAsync([id], cancellationToken);
        }

        public IQueryable<Auction> AuctionsQuery => Set<Auction>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LiveBidDbContext).Assembly);
        }
    }
}
