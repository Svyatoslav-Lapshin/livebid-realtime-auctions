using LiveBid.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Domain.Auctions
{
    public sealed class Bid: BaseEntity
    {
        public Guid AuctionId { get; set; }
        public Guid BidderId { get; set; }
        public decimal Amount { get; set; }

        public DateTimeOffset? PlacedAt { get; set; }=DateTimeOffset.UtcNow;

        public Auction Auction { get; set; } = null!;

    }
}
