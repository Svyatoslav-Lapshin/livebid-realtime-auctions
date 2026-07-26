using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Application.Features.Auctions.PlaceBid
{
   public sealed record PlaceBidResponse(Guid AuctionId, Guid BidderId,Guid BidId, decimal BidAmount, decimal CurrentPrice, DateTimeOffset PlacedAt);
}
