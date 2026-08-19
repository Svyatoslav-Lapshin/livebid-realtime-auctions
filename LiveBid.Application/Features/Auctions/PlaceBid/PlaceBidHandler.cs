using FluentValidation;
using LiveBid.Application.Common;
using LiveBid.Application.Common.Interfaces;
using LiveBid.Application.Features.Common;
using LiveBid.Domain.Auctions;

namespace LiveBid.Application.Features.Auctions.PlaceBid
{
    public sealed class PlaceBidHandler(ILiveBidDbContext dbContext, IValidator<PlaceBidCommand> validator)
    {
        private readonly ILiveBidDbContext _dbContext = dbContext;
        private readonly IValidator<PlaceBidCommand> _validator = validator;

        public async Task<Result<PlaceBidResponse>> Handle(PlaceBidCommand command, CancellationToken cancellationToken)
        {

            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result<PlaceBidResponse>.Failure(new Error("ValidationError", errorMessage));
            }

            var auction = await _dbContext.GetAuctionAsync(command.AuctionId, cancellationToken);

            if (auction is null)
            {
                return Result<PlaceBidResponse>.Failure(AuctionErrors.NotFound);
            }

            var bidExists = await _dbContext.UserExistAsync(command.BidderId, cancellationToken);

            if (!bidExists)
            {
                return Result<PlaceBidResponse>.Failure(AuctionErrors.BidderNotFound);
            }

            var currentTime = DateTimeOffset.UtcNow;

            if (auction.Status is AuctionStatus.Ended)
            {
                return Result<PlaceBidResponse>.Failure(AuctionErrors.AuctionEnded);
            }

            if (auction.Status is not AuctionStatus.Live)
            {
                return Result<PlaceBidResponse>.Failure(AuctionErrors.AuctionNotLive);
            }


            if (currentTime < auction.StartTime || currentTime >= auction.EndTime)
            {
                return Result<PlaceBidResponse>.Failure(AuctionErrors.AuctionNotActive);
            }


            if (auction.SellerId == command.BidderId)
            {
                return Result<PlaceBidResponse>.Failure(AuctionErrors.SellerCannotBid);
            }


            if (command.BidAmount <= auction.CurrentPrice)
            {
                return Result<PlaceBidResponse>.Failure(AuctionErrors.BidTooLow);
            }

            var bid = auction.PlaceBid(command.BidderId, command.BidAmount, currentTime);

            if (bid is null)
            {
                return Result<PlaceBidResponse>.Failure(AuctionErrors.CannotPlaceBid);
            }

            await _dbContext.AddBidAsync(bid,cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result<PlaceBidResponse>.Success(new PlaceBidResponse(bid.Id, auction.Id, bid.BidderId, bid.Amount, auction.CurrentPrice, currentTime));
        }
    }
}
