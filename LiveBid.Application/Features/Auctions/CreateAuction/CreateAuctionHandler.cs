using FluentValidation;
using LiveBid.Application.Common;
using LiveBid.Application.Common.Interfaces;
using LiveBid.Domain.Auctions;


namespace LiveBid.Application.Features.Auctions.CreateAuction
{
    public sealed class CreateAuctionHandler(ILiveBidDbContext dbContext, IValidator<CreateAuctionCommand> validator)
    {
        private readonly ILiveBidDbContext _dbContext = dbContext;
        private readonly IValidator<CreateAuctionCommand> _validator = validator;

       
        // This method handles the creation of a new auction based on the provided command.
        public async Task<Result<CreateAuctionResponse>> Handle(CreateAuctionCommand command, CancellationToken cancellationToken)
        {
            // Validate the command using the provided validator
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            // If the validation fails, return a failure result with the validation errors
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result<CreateAuctionResponse>.Failure(new Error("ValidationError", errorMessage));
            }
            // Create a new Auction entity based on the command data
            var auction = new Auction
            {
                SellerId = command.SellerId,
                Title = command.Title,
                Description = command.Description,
                StartPrice = command.StartPrice,
                CurrentPrice = command.StartPrice,
                StartTime = command.StartTime,
                EndTime = command.EndTime,
                Status = AuctionStatus.Scheduled
            };

            // Add the new auction to the database context and save changes
            await _dbContext.AddAuctionAsync(auction, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Return a success result with the created auction details
            return Result<CreateAuctionResponse>.Success(new CreateAuctionResponse
            {
                AuctionId = auction.Id,
                Title = auction.Title,
                Description = auction.Description,
                StartPrice = auction.StartPrice,
                CurrentPrice=auction.CurrentPrice,
                StartTime = auction.StartTime,
                EndTime = auction.EndTime,
                Status = auction.Status.ToString()
            });
        }

    }
}
