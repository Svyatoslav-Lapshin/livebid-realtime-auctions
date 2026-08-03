using FluentValidation;
using LiveBid.Application.Features.Auctions.CancelAuction;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Application.Features.Auctions.StartAuction
{
    public sealed class StartAuctionValidator : AbstractValidator<StartAuctionCommand>
    {
        public StartAuctionValidator()
        {
            RuleFor(x => x.AuctionId).NotEmpty().WithMessage("Auction ID is required.");

        }

    }
}
