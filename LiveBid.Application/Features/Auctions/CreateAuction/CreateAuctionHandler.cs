using FluentValidation;
using LiveBid.Application.Common;
using LiveBid.Application.Common.Interfaces;
using LiveBid.Domain.Auctions;


namespace LiveBid.Application.Features.Auctions.CreateAuction
{
    public sealed class CreateAuctionHandler 
    {
        private readonly ILiveBidDbContext _dbContext;
        private readonly IValidator<CreateAuctionCommand> _validator;

        public CreateAuctionHandler(ILiveBidDbContext dbContext, IValidator<CreateAuctionCommand> validator)
        {
            _dbContext = dbContext;
            _validator = validator;
        }

        public async Task<Result<CreateAuctionResponse>> Handle(CreateAuctionCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result<CreateAuctionResponse>.Failure(new Error("ValidationError", errorMessage));
            }

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

            await _dbContext.AddAuctionAsync(auction, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

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
