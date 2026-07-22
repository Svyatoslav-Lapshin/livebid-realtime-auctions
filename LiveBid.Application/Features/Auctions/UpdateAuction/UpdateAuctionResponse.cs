using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Application.Features.Auctions.UpdateAuction
{
    public sealed record UpdateAuctionResponse
    (
       Guid AuctionId,
       string Title,
       string Description,
       decimal StartPrice,
       DateTimeOffset StartTime,
       DateTimeOffset EndTime,
       string Status
    );
}
