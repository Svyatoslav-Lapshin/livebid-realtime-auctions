using LiveBid.Application.Common;
using LiveBid.Application.Common.Interfaces;

namespace LiveBid.Application.Features.Auctions.GetAuctionById
{
    public sealed class GetAuctionByIdHandler (ILiveBidDbContext dbContext)
    
    {
        private readonly ILiveBidDbContext _dbContext=dbContext;

      
        // This method handles the retrieval of an auction by its ID.
        public async Task<Result<GetAuctionByIdResponse>> Handle(GetAuctionByIdQuery query, CancellationToken cancellationToken)
        {
            // Retrieve the auction from the database using the provided auction ID.
            var auction = await _dbContext.GetAuctionAsync(query.AuctionId, cancellationToken);

            // If the auction is not found, return a failure result with an appropriate error message.
            if (auction is null)
            {
                return Result<GetAuctionByIdResponse>.Failure(new Error(
                "Auction.NotFound",
                $"Auction with ID {query.AuctionId} was not found."));

            }

            // If the auction is found, create a response object with the auction details.
            var response = new GetAuctionByIdResponse
            {
                AuctionId = auction.Id,
                SellerId = auction.SellerId,
                Title = auction.Title,
                Description = auction.Description,
                StartPrice = auction.StartPrice,
                CurrentPrice = auction.CurrentPrice,
                StartDate = auction.StartTime,
                EndDate = auction.EndTime,
                Status = auction.Status.ToString()
            };

            // Return a success result with the response object.
            return Result<GetAuctionByIdResponse>.Success(response);
        }


    }
}
