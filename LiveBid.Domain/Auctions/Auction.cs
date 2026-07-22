using LiveBid.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Domain.Auctions
{
    public sealed class Auction:BaseEntity
    {
        public Guid SellerId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; }= string.Empty;

        public decimal StartPrice { get; set; }
        public decimal CurrentPrice { get; set; }

        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }

        public AuctionStatus Status { get; set; } = AuctionStatus.Draft;

        public List<Bid> Bids { get; set; } = [];


        public void Update(string title, string description,decimal startPrice, DateTimeOffset startTime, DateTimeOffset endTime)
        {
            Title = title;
            Description = description;
            StartPrice = startPrice;
            CurrentPrice = startPrice;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
