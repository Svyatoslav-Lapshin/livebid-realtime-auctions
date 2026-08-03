using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Application.Features.Auctions.EndAuction
{
    public sealed record EndAuctionCommand(Guid AuctionId);
}
