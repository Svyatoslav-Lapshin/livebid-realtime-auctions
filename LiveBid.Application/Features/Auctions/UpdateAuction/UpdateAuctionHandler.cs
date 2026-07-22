using LiveBid.Application.Common;
using LiveBid.Application.Common.Interfaces;
using LiveBid.Application.Features.Common;
using LiveBid.Domain.Auctions;
using System;
using System.Collections.Generic;
using System.Text;


namespace LiveBid.Application.Features.Auctions.UpdateAuction
{
    public sealed class UpdateAuctionHandler(ILiveBidDbContext dbContext)
    {
        private readonly ILiveBidDbContext _dbContext = dbContext;

        public async Task<Result<UpdateAuctionResponse>> Handle(UpdateAuctionCommand command, CancellationToken cancellationToken)
        {
            var auction = await _dbContext.GetAuctionAsync(command.AuctionId, cancellationToken);

            if (auction is null)
            {
                return Result<UpdateAuctionResponse>.Failure(AuctionErrors.NotFound);
            }

            if (auction.Status is not AuctionStatus.Draft and not AuctionStatus.Scheduled)
            {
                return Result<UpdateAuctionResponse>.Failure(AuctionErrors.CannotUpdate);
            }

            if (auction.StartTime <= DateTimeOffset.UtcNow)
            {
                return Result<UpdateAuctionResponse>.Failure(AuctionErrors.CannotUpdate);

            }

            auction.Update(command.Title, command.Description, command.StartPrice, command.StartTime, command.EndTime);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<UpdateAuctionResponse>.Success(new UpdateAuctionResponse
            (
                    auction.Id,
                    auction.Title,
                    auction.Description,
                    auction.StartPrice,
                    auction.StartTime,
                    auction.EndTime,
                    auction.Status.ToString()
            ));
        } 
    
    }
}
