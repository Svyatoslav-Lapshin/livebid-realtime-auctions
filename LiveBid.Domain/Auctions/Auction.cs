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

        public AuctionStatus Status { get; private set; } = AuctionStatus.Draft;

        public List<Bid> Bids { get; set; } = [];


        private Auction()
        {
            
        }


        public Auction(Guid sellerId, string title, string description, decimal startPrice, DateTimeOffset startTime, DateTimeOffset endTime)
        {
            SellerId = sellerId;
            Title = title;
            Description = description;
            StartPrice = startPrice;
            CurrentPrice = startPrice;
            StartTime = startTime;
            EndTime = endTime;
            Status = AuctionStatus.Draft;
        }

        public void Update(string title, string description,decimal startPrice, DateTimeOffset startTime, DateTimeOffset endTime)
        {
            Title = title;
            Description = description;
            StartPrice = startPrice;
            CurrentPrice = startPrice;
            StartTime = startTime;
            EndTime = endTime;
        }

        public bool Schedule(DateTimeOffset currentTime)
        {
            if (Status != AuctionStatus.Draft)
            {
                return false;
            }
            if (StartTime <= currentTime || EndTime <= StartTime)
            {
                return false;
            }
            Status = AuctionStatus.Scheduled;
            return true;
        }

        public bool Cancel(DateTimeOffset currentTime)
        {
            if (Status is AuctionStatus.Draft and not AuctionStatus.Scheduled)
            {
                return false;
            }

            if (Status == AuctionStatus.Scheduled && StartTime<=currentTime)
            {
                return false;
            }

            Status = AuctionStatus.Canceled;
            return true;
        }
    }
}
