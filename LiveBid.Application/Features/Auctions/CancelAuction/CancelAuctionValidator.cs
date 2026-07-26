using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Application.Features.Auctions.CancelAuction
{
    public sealed class CancelAuctionValidator: AbstractValidator<CancelAuctionCommand>
    {
        public CancelAuctionValidator() { 
                RuleFor(x => x.AuctionId).NotEmpty().WithMessage("Auction ID is required.");
               
        }

    }
}
