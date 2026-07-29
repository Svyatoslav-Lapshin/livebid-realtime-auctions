using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Application.Features.Auctions.PlaceBid
{
    public sealed record PlaceBidResponse(
     Guid BidId,
     Guid AuctionId,
     Guid BidderId,
     decimal BidAmount,
     decimal CurrentPrice,
     DateTimeOffset PlacedAt);
}
