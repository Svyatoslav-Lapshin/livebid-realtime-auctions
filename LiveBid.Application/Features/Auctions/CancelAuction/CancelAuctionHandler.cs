using FluentValidation;
using LiveBid.Application.Common;
using LiveBid.Application.Common.Interfaces;
using LiveBid.Application.Features.Common;


namespace LiveBid.Application.Features.Auctions.CancelAuction
{
    public sealed class CancelAuctionHandler(ILiveBidDbContext dbContext, IValidator<CancelAuctionCommand> validator)
    {
        private readonly ILiveBidDbContext _dbContext = dbContext;
        private readonly IValidator<CancelAuctionCommand> _validator = validator;

        public async Task<Result<CancelAuctionResponse>> Handle(CancelAuctionCommand command, CancellationToken cancellationToken)
        {
            // Validate the command using the provided validator
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            // If the validation fails, return a failure result with the validation errors
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result<CancelAuctionResponse>.Failure(new Error("ValidationError", errorMessage));
            }

            var auction = await _dbContext.GetAuctionAsync(command.AuctionId, cancellationToken);

            if (auction is null)
            {
                return Result<CancelAuctionResponse>.Failure(AuctionErrors.NotFound);
            }

            var wasCancelled = auction.Cancel(DateTimeOffset.UtcNow);
            if (!wasCancelled) { 
                    return Result<CancelAuctionResponse>.Failure(AuctionErrors.CannotCancel);

            }
          
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<CancelAuctionResponse>.Success(new CancelAuctionResponse
            (
                    auction.Id,
                    auction.Status.ToString()
            ));
        }
    }
}
