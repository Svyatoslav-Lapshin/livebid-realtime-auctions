using LiveBid.Application.Common;
using Microsoft.AspNetCore.Http;
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

        public static readonly Error CannotEnd = new Error(
            "Auction.CannotEnd",
            "The auction cannot be ended in its current state or before its end time."
            );

        public static readonly Error CannotStart = new(
            "Auction.CannotStart",
            "The auction cannot be started because it is either closed or has already started."
            );

        public static readonly Error CannotUpdate = new(
            "Auction.CannotUpdate",
            "The auction cannot be updated because it is either closed or has already started."
        );

        public static readonly Error CannotSchedule = new(
            "Auction.CannotSchedule",
            "The auction cannot be scheduled because it is either closed or has already started."
            );

        public static readonly Error CannotCancel = new(
            "Auction.CannotCancel",
            "The auction cannot be canceled because it is either closed or has already started."
        );

        public static readonly Error CannotPlaceBid = new(
            "Auction.CannotPlaceBid",
            "The auction cannot accept bids because it is either closed or has not started yet."
        );

        public static readonly Error AuctionNotActive = new(
            "Auction.NotActive",
            "The auction is not currently active. Bids can only be placed on active auctions."

            );
        public static readonly Error AuctionEnded = new(
            "Auction.Ended",
            "The auction has ended and no longer accepts bids."
        );

        public static readonly Error AuctionNotFound = new(
         "Auction.NotFound",
         "Auction was not found.");

        public static readonly Error BidderNotFound = new(
            "Bidder.NotFound",
            "Bidder was not found.");

        public static readonly Error AuctionNotLive = new(
            "Auction.NotLive",
            "Bids can only be placed on a live auction.");

        public static readonly Error SellerCannotBid = new(
            "Auction.SellerCannotBid",
            "Seller cannot place a bid on their own auction.");

        public static readonly Error BidTooLow = new(
            "Bid.TooLow",
            "Bid amount must be greater than the current price.");
    }
}
