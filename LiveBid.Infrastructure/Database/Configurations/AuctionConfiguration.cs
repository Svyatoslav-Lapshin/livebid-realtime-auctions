using LiveBid.Domain.Auctions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiveBid.Infrastructure.Database.Configurations
{
    public sealed class AuctionConfiguration : IEntityTypeConfiguration<Auction>
    {
        public void Configure(EntityTypeBuilder<Auction> builder)
        {
            builder.ToTable("auctions");

            builder.HasKey(auction => auction.Id);

            builder.Property(auction => auction.Id).HasColumnName("id").ValueGeneratedOnAdd();

            builder.Property(auction => auction.SellerId).HasColumnName("seller_id").IsRequired();

            builder.Property(auction => auction.Title).HasColumnName("title").IsRequired().HasMaxLength(200);

            builder.Property(auction => auction.Description).HasColumnName("description").IsRequired();

            builder.Property(auction => auction.StartPrice).HasColumnName("start_price").HasPrecision(18, 2).IsRequired();

            builder.Property(auction => auction.CurrentPrice).HasColumnName("current_price").HasPrecision(18, 2).IsRequired();

            builder.Property(auction => auction.StartTime).HasColumnName("start_time").IsRequired();

            builder.Property(auction => auction.EndTime).HasColumnName("end_time").IsRequired();

            builder.Property(auction => auction.Status).HasColumnName("status").HasConversion(status=>status.ToString().ToLowerInvariant(), value=> Enum.Parse<AuctionStatus>(value, ignoreCase:true)).HasMaxLength(30).IsRequired();

            builder.Property(auction => auction.CreatedAt).HasColumnName("created_at").IsRequired();

            builder.Property(auction => auction.UpdatedAt).HasColumnName("updated_at");

            builder.HasMany(auction => auction.Bids)
                .WithOne(bid => bid.Auction)
                .HasForeignKey(bid => bid.AuctionId);
           

        }
    }
}
