using LiveBid.Application.Features.Auctions.GetLiveAuctions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Application.Features.Auctions.GetAuctionBids
{
    public sealed record GetAuctionBidsResponse(Guid AuctionId, IReadOnlyList<AuctionBidResponse> Items, int TotalCount, int Page, int PageSize);
   
}
