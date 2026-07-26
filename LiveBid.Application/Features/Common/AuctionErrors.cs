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

        public static readonly Error CannotSchedule=new(
            "Auction.CannotSchedule",
            "The auction cannot be scheduled because it is either closed or has already started."
            );

        public static readonly Error CannotCancel = new(
            "Auction.CannotCancel",
            "The auction cannot be canceled because it is either closed or has already started."
        );
    }
}
