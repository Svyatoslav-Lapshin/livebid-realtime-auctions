using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Application.Features.Auctions.EndAuction
{
    public sealed record EndAuctionResponse(Guid AuctionId, string Status, decimal FinalPrice, Guid? WinnerId, Guid? WinningBid);
}
