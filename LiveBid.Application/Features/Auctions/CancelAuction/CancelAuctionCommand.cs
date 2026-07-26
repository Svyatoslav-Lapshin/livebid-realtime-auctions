using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Application.Features.Auctions.CancelAuction
{
    public sealed record CancelAuctionCommand(Guid AuctionId);


}