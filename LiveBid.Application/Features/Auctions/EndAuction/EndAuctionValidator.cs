using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Application.Features.Auctions.EndAuction
{
    public sealed class EndAuctionValidator : AbstractValidator<EndAuctionCommand>
    {
        public EndAuctionValidator()
        {
            RuleFor(x => x.AuctionId).NotEmpty().WithMessage("Auction ID is required.");

        }

    }
}
