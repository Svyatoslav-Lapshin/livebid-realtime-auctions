using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LiveBid.Application.Features.Auctions.PlaceBid
{
    public sealed class PlaceBidValidator : AbstractValidator<PlaceBidCommand>
    {

        public PlaceBidValidator()
        {
            RuleFor(x => x.AuctionId).NotEmpty().WithMessage("Auction ID is required.");

            RuleFor(x => x.BidderId).NotEmpty().WithMessage("Bidder ID is required.");

            RuleFor(x => x.BidAmount).GreaterThan(0m).WithMessage("Bid amount must be greater than zero.");

        }

    }
}
