using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Application.Features.Auctions.GetAuctionBids
{
    public sealed record GetAuctionBidsQuery(Guid AuctionId, int Page = 1, int PageSize = 20, string? SortDirection = "desc");
  

}
