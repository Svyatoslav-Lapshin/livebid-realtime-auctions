using FluentValidation;

namespace LiveBid.Application.Features.Auctions.CreateAuction
{
   public sealed  class CreateAuctionValidator: AbstractValidator<CreateAuctionCommand>
    {
        // This class defines validation rules for the CreateAuctionCommand using FluentValidation.
        public CreateAuctionValidator() {


            RuleFor(x => x.SellerId).NotEmpty();

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(250).WithMessage("Description must not exceed 250 characters.").NotEmpty().WithMessage("Description is required.");

            RuleFor(x => x.StartPrice)
                .GreaterThan(0).WithMessage("Start price must be greater than 0.");

            RuleFor(x => x.StartTime)
                .GreaterThan(DateTimeOffset.UtcNow).WithMessage("Start time must be in the future.");

            RuleFor(x => x.EndTime)
                .GreaterThan(x => x.StartTime).WithMessage("End time must be after the start time.");




        }
    }
}
