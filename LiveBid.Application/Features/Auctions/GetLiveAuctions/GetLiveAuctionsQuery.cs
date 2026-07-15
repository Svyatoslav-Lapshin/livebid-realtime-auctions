using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Application.Features.Auctions.GetLiveAuctions
{
    public sealed record GetLiveAuctionsQuery(int Page=1, int PageSize=20, string? Search = null, string? Status = null, string? SortDirection= null);
    
}
