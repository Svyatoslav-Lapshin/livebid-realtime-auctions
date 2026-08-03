using LiveBid.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Domain.Auctions
{
    public sealed class Auction : BaseEntity
    {
        public Guid SellerId { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public decimal StartPrice { get; private set; }
        public decimal CurrentPrice { get; private set; }

        public DateTimeOffset StartTime { get; private set; }
        public DateTimeOffset EndTime { get; private set; }

        public AuctionStatus Status { get; private set; } = AuctionStatus.Draft;

        public List<Bid> Bids { get; private set; } = [];


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

        public void Update(string title, string description, decimal startPrice, DateTimeOffset startTime, DateTimeOffset endTime)
        {
            Title = title;
            Description = description;
            StartPrice = startPrice;
            CurrentPrice = startPrice;
            StartTime = startTime;
            EndTime = endTime;
        }


        public bool Start(DateTimeOffset currentTime)
        {
            if (Status != AuctionStatus.Scheduled)
            {
                return false;

            }

            if (currentTime < StartTime)
            {
                return false;
            }

            if (currentTime >= EndTime)
            {
                return false;
            }

            Status = AuctionStatus.Live;

            return true;
        }

        public bool End(DateTimeOffset currentTime)
        {
            if (Status != AuctionStatus.Live)
            {
                return false;
            }

            if (currentTime < EndTime)
            {
                return false;
            }

            Status = AuctionStatus.Ended;
            return true;
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
            if (Status is not AuctionStatus.Draft and not AuctionStatus.Scheduled)
            {
                return false;
            }

            if (Status == AuctionStatus.Scheduled && StartTime <= currentTime)
            {
                return false;
            }

            Status = AuctionStatus.Canceled;
            return true;
        }


        public Bid? PlaceBid(Guid bidderId, decimal bidAmount, DateTimeOffset currentTime)
        {
            if (Status != AuctionStatus.Live)
            {
                return null;
            }

            if (currentTime < StartTime || currentTime >= EndTime)
            {
                return null;
            }

            if (bidderId == SellerId)
            {
                return null;
            }

            if (bidAmount <= CurrentPrice)
            {
                return null;
            }

            var bid = new Bid(Id, bidderId, bidAmount, currentTime);
            Bids.Add(bid);
            CurrentPrice = bidAmount;
            return bid;
        }
    }
}
