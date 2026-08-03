using FluentValidation;
using LiveBid.Application.Common;
using LiveBid.Application.Common.Interfaces;
using LiveBid.Application.Features.Common;


namespace LiveBid.Application.Features.Auctions.EndAuction
{
    public sealed class EndAuctionHandler(ILiveBidDbContext dbContext, IValidator<EndAuctionCommand> validator)
    {
        private readonly ILiveBidDbContext _dbContext = dbContext;
        private readonly IValidator<EndAuctionCommand> _validator = validator;

        public async Task<Result<EndAuctionResponse>> Handle(EndAuctionCommand command, CancellationToken cancellationToken)
        {
            // Validate the command using the provided validator
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            // If the validation fails, return a failure result with the validation errors
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result<EndAuctionResponse>.Failure(new Error("ValidationError", errorMessage));
            }

            var auction = await _dbContext.GetAuctionAsync(command.AuctionId, cancellationToken);
            if (auction is null)
            {
                return Result<EndAuctionResponse>.Failure(AuctionErrors.NotFound);
            }

            var currentTime = DateTimeOffset.UtcNow;

            var wasEnded = auction.End(currentTime);
            if (!wasEnded)
            {
                return Result<EndAuctionResponse>.Failure(AuctionErrors.CannotEnd);
            }

            var winningBid= await _dbContext.GetWinningBidAsync(auction.Id, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result<EndAuctionResponse>.Success(new EndAuctionResponse
            (
                auction.Id,
                auction.Status.ToString(),
                winningBid?.Amount ?? auction.CurrentPrice,
                winningBid?.BidderId,
                winningBid?.Id
            ));
        }
    }
}
