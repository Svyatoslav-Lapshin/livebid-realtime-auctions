using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Application.Features.Auctions.StartAuction
{
    public sealed record StartAuctionCommand(Guid AuctionId);
}
