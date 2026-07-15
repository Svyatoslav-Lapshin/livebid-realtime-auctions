using LiveBid.Application.Common.Interfaces;
using LiveBid.Application.Features.Auctions.GetAuctionById;
using LiveBid.Domain.Auctions;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Text;
using LiveBid.Application.Common;

namespace LiveBid.Application.Features.Auctions.GetLiveAuctions
{
    public sealed class GetLiveAuctionsHandler(ILiveBidDbContext dbContext)
    {
        private readonly ILiveBidDbContext _dbContext = dbContext;

        public async Task<Result<GetLiveAuctionsResponse>> Handle(GetLiveAuctionsQuery query, CancellationToken cancellationToken=default)
        {
            var page=query.Page < 1 ? 1 : query.Page;
            var pageSize = Math.Clamp(query.PageSize, 1, 100);

            var auctionsQuery = _dbContext.AuctionsQuery
                .AsNoTracking()
                .Where(a => a.Status == AuctionStatus.Live);

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.Trim().ToLower();

                auctionsQuery = auctionsQuery.Where(a =>
                    a.Title.ToLower().Contains(search) ||
                    a.Description.ToLower().Contains(search));
            }


            var totalCount = await _dbContext.AuctionsQuery.CountAsync(a => a.Status == AuctionStatus.Live, cancellationToken);

            auctionsQuery = query.SortDirection?.ToLower() switch
            {
                "asc" => auctionsQuery.OrderBy(a => a.StartTime),
                "desc" => auctionsQuery.OrderByDescending(a => a.StartTime),
                _ => auctionsQuery.OrderByDescending(a => a.StartTime)
            };

            var auctions = await auctionsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new GetLiveAuctionItemResponse
                {
                    AuctionId = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    CurrentPrice = a.CurrentPrice,
                    EndTime = a.EndTime,
                    Status = a.Status.ToString(),
                  
                })
                .ToListAsync(cancellationToken);

            var response= new GetLiveAuctionsResponse
            {
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                Auctions = auctions
            };

            return Result<GetLiveAuctionsResponse>.Success(response);
        }

    }
}
