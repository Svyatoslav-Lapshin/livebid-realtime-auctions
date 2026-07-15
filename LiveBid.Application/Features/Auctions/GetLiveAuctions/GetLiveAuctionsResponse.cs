using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Application.Features.Auctions.GetLiveAuctions
{
    public sealed class GetLiveAuctionsResponse
    {
        public int Page { get; init; }
        public int PageSize { get; init; }
        public int TotalCount { get; set; }
        public int MyProperty { get; set; }

        public IReadOnlyList<GetLiveAuctionItemResponse> Auctions { get; init; } = [];


    }




}