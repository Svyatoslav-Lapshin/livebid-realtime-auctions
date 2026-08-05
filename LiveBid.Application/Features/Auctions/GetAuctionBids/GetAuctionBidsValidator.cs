using FluentValidation;
using LiveBid.Application.Features.Auctions.EndAuction;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Application.Features.Auctions.GetAuctionBids
{
    public sealed class GetAuctionBidsValidator : AbstractValidator<GetAuctionBidsQuery>
    {
        public GetAuctionBidsValidator() { 
        
            RuleFor(x => x.AuctionId).NotEmpty().WithMessage("Auction ID is required.");

            RuleFor(x => x.Page).GreaterThan(0).WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.PageSize).InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");

            RuleFor(x => x.SortDirection)
             .Must(direction =>
                 string.Equals(
                     direction,
                     "asc",
                     StringComparison.OrdinalIgnoreCase) ||
                 string.Equals(
                     direction,
                     "desc",
                     StringComparison.OrdinalIgnoreCase))
             .WithMessage(
                 "Sort direction must be either 'asc' or 'desc'.");


        }

    }
}
