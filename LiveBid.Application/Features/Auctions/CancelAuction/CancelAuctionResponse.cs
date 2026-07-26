
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Application.Features.Auctions.CancelAuction
{
    public sealed record CancelAuctionResponse(Guid AuctionId, string Status);
}
