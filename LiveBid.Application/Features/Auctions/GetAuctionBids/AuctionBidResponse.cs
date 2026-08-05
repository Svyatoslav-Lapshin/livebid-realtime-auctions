using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Application.Features.Auctions.GetAuctionBids
{
    public sealed record AuctionBidResponse(Guid BidId, Guid BidderId, decimal Amount, DateTimeOffset PlacedAt);
   
}
