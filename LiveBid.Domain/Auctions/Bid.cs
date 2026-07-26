using LiveBid.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Domain.Auctions
{
    public sealed class Bid: BaseEntity
    {
        private Bid() { }

        public Bid(Guid auctionId, Guid bidderId, decimal amount, DateTimeOffset placedAt)
        {
            AuctionId = auctionId;
            BidderId = bidderId;
            Amount = amount;
            PlacedAt = placedAt;
        }


        public Guid AuctionId { get; private set; }
        public Guid BidderId { get; private set; }
        public decimal Amount { get; private set; }
        public DateTimeOffset PlacedAt { get; private set; }
        public Auction Auction { get; private set; } = null!;

    }
}
