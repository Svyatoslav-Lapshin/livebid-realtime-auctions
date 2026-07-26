using LiveBid.Domain.Auctions;
using LiveBid.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiveBid.Infrastructure.Database.Configurations
{
    public sealed class BidConfiguration : IEntityTypeConfiguration<Bid>
    {
        public void Configure(EntityTypeBuilder<Bid> builder)
        {
            builder.ToTable("bids");

            builder.HasKey(bid => bid.Id);

            builder.Property(bid => bid.Id)
                .HasColumnName("id");
              

            builder.Property(bid => bid.AuctionId)
                .HasColumnName("auction_id")
                .IsRequired();

            builder.Property(bid => bid.BidderId)
                .HasColumnName("bidder_id")
                .IsRequired();

            builder.Property(bid => bid.Amount)
                .HasColumnName("amount")
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(bid => bid.PlacedAt)
                .HasColumnName("placed_at")
                .IsRequired();

            builder.Property(bid => bid.CreatedAt)
                .HasColumnName("created_at")        
                .IsRequired();

            builder.Property(bid => bid.UpdatedAt)
                .HasColumnName("updated_at");

            builder.HasOne(bid => bid.Auction)
                .WithMany(auction => auction.Bids)
                .HasForeignKey(bid => bid.AuctionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(bid => bid.BidderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(bid => bid.BidderId)
                .HasDatabaseName("idx_bids_bidder_id");

            builder.HasIndex(bid => new
            {
                bid.AuctionId,
                bid.PlacedAt
            })
                .HasDatabaseName("idx_bids_auction_id_placed_at");


        }
    }
}