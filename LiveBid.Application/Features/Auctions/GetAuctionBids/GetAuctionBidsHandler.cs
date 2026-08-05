using FluentValidation;
using LiveBid.Application.Common;
using LiveBid.Application.Common.Interfaces;
using LiveBid.Application.Features.Common;
using Microsoft.EntityFrameworkCore;

namespace LiveBid.Application.Features.Auctions.GetAuctionBids
{
    public sealed class GetAuctionBidsHandler(ILiveBidDbContext dbContext, IValidator<GetAuctionBidsQuery> validator)
    {
        private readonly ILiveBidDbContext _dbContext = dbContext;
        private readonly IValidator<GetAuctionBidsQuery> _validator = validator;

        public async Task<Result<GetAuctionBidsResponse>> Handle(GetAuctionBidsQuery query, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(query, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errorMessages =string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result<GetAuctionBidsResponse>.Failure(new Error("ValidationError",errorMessages));
            }

            var auctionExists = await _dbContext.AuctionsQuery
                .AnyAsync(a => a.Id == query.AuctionId, cancellationToken);





            if (!auctionExists)
            {
                return Result<GetAuctionBidsResponse>.Failure(
               AuctionErrors.NotFound);
            }
        
            var bidsQuery = _dbContext.BidsQuery            
                .Where(b => b.AuctionId == query.AuctionId);

            var totalCount = await bidsQuery.CountAsync(cancellationToken);

            bidsQuery = query.SortDirection?.ToLowerInvariant() switch
            {
                "asc" => bidsQuery.OrderBy(b => b.PlacedAt).ThenBy(b => b.Id),
                _ => bidsQuery.OrderByDescending(b => b.PlacedAt).ThenByDescending(b => b.Id)
            };

            var bids = await bidsQuery
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(b => new AuctionBidResponse(
                    b.Id,
                    b.BidderId,
                    b.Amount,
                    b.PlacedAt))
                .ToListAsync(cancellationToken);

            var response = new GetAuctionBidsResponse(
                query.AuctionId,
                bids,
                totalCount,
                query.Page,
                query.PageSize);

            return Result<GetAuctionBidsResponse>.Success(response);
        }

    }
}
