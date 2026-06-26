using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Domain.Auctions
{
   public enum AuctionStatus
    {
        Draft=1,
        Scheduled = 2,
        Live = 3,
        Ended = 4,
        Canceled = 5,
    }
}
