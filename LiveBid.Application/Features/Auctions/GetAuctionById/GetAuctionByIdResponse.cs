using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Application.Features.Auctions.GetAuctionById
{
    public sealed class GetAuctionByIdResponse
    {
        public Guid AuctionId { get; init; }
        public Guid SellerId { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public decimal StartPrice { get; init; } 
        public decimal CurrentPrice { get; init; }

        public DateTimeOffset StartDate { get; init; }
        public DateTimeOffset EndDate { get; init; }

        public string Status { get; init; } = string.Empty;
    }
}
