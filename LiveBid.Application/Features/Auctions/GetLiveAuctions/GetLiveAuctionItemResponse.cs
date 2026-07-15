using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Application.Features.Auctions.GetLiveAuctions
{
    public sealed class GetLiveAuctionItemResponse
    {
        public Guid AuctionId { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public decimal CurrentPrice { get; init; }
        public DateTimeOffset EndTime { get; init; }
        public string Status { get; init; } = string.Empty;


    }
}
