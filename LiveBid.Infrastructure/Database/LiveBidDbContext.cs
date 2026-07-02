using LiveBid.Domain.Auctions;
using LiveBid.Domain.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Infrastructure.Database
{
    public class LiveBidDbContext:DbContext
    {
        public LiveBidDbContext(DbContextOptions<LiveBidDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        public DbSet<Auction> Auctions => Set<Auction>();

        public DbSet<Bid> Bids => Set<Bid>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LiveBidDbContext).Assembly);
        }
    }
}
