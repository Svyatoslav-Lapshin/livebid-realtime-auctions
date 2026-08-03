using FluentValidation;
using LiveBid.Application.Common;
using LiveBid.Application.Common.Interfaces;
using LiveBid.Application.Features.Common;

namespace LiveBid.Application.Features.Auctions.StartAuction
{
    public sealed class StartAuctionHandler (ILiveBidDbContext dbContext, IValidator<StartAuctionCommand> validator)
    {
        private readonly ILiveBidDbContext _dbContext = dbContext;
        private readonly IValidator<StartAuctionCommand> _validator = validator;

        public async Task<Result<StartAuctionResponse>> Handle(StartAuctionCommand command, CancellationToken cancellationToken)
        {
            // Validate the command using the provided validator
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            // If the validation fails, return a failure result with the validation errors
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result<StartAuctionResponse>.Failure(new Error("ValidationError", errorMessage));
            }

            var auction = await _dbContext.GetAuctionAsync(command.AuctionId, cancellationToken);
            if (auction is null)
            {
                return Result<StartAuctionResponse>.Failure(AuctionErrors.NotFound);
            }

            var currentTime = DateTimeOffset.UtcNow;

            var wasStarted = auction.Start(currentTime);
            if (!wasStarted)
            {
                return Result<StartAuctionResponse>.Failure(AuctionErrors.CannotStart);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result<StartAuctionResponse>.Success(new StartAuctionResponse
            (
                auction.Id,
                auction.Status.ToString()
            ));
        }

    }
}
