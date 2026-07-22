using LiveBid.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Application.Features.Common
{
    public static class AuctionErrors
    {
        public static readonly Error NotFound = new
        (
           "Auction.NotFound",
           "The auction with the specified ID was not found."
        );


        public static readonly Error CannotUpdate = new(
            "Auction.CannotUpdate",
            "The auction cannot be updated because it is either closed or has already started."
        );
    }
}
