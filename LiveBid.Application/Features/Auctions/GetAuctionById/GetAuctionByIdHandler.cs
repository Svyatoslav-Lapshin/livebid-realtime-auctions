using LiveBid.Application.Common;
using LiveBid.Application.Common.Interfaces;

namespace LiveBid.Application.Features.Auctions.GetAuctionById
{
    public sealed class GetAuctionByIdHandler
    {
        private readonly ILiveBidDbContext _dbContext;
     

        public GetAuctionByIdHandler(ILiveBidDbContext dbContext)
        {
            _dbContext = dbContext;
          
        }

        public async Task<Result<GetAuctionByIdResponse>> Handle(GetAuctionByIdQuery query, CancellationToken cancellationToken)
        {
           

            var auction = await _dbContext.GetAuctionAsync(query.AuctionId, cancellationToken);

            if (auction is  null)
            {
                return Result<GetAuctionByIdResponse>.Failure(new Error(
                "Auction.NotFound",
                  $"Auction with ID {query.AuctionId} was not found."));

            }

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

            return Result<GetAuctionByIdResponse>.Success(response);
        }


    }
}
