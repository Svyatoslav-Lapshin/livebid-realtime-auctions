using LiveBid.Domain.Auctions;
using LiveBid.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Infrastructure.Database.Configurations
{
    public sealed class BidConfiguration : IEntityTypeConfiguration<Bid>
    {
        public void Configure(EntityTypeBuilder<Bid> builder)
        {
            builder.ToTable("bids");

            builder.HasKey(bid => bid.Id);

            builder.Property(bid => bid.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");


            builder.Property(bid => bid.AuctionId).HasColumnName("auction_id");

            builder.Property(auction => auction.BidderId).HasColumnName("bidder_id");

            builder.Property(auction => auction.Amount).HasColumnName("amount").HasPrecision(18, 2).IsRequired();


            builder.Property(auction => auction.PlacedAt).HasColumnName("placed_at").HasDefaultValueSql("now").IsRequired();

            builder.Property(auction => auction.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()").IsRequired();

            builder.Property(auction => auction.UpdatedAt).HasColumnName("updated_at");



            builder.HasOne(bid => bid.Auction)
                 .WithMany(auction => auction.Bids)
                 .HasForeignKey(auction => auction.AuctionId);

            builder.HasOne<User>()
             .WithMany()
            .HasForeignKey(bid => bid.BidderId);

            builder.HasIndex(bid => bid.AuctionId)
                .HasDatabaseName("idx_bids_auction_id");

            builder.HasIndex(bid => bid.BidderId)
                .HasDatabaseName("idx_bids_bidder_id");

            builder.HasIndex(bid => new { bid.AuctionId, bid.PlacedAt })
                .HasDatabaseName("idx_bids_auction_id_placed_at");


        }
    }
}