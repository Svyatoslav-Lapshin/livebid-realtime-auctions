using LiveBid.Application.Features.Auctions.PlaceBid;

namespace realtime_auction_platform.EndPoints.Auctions
{
    public static  class PlaceBidEndpoint
    {
        public static void MapPlaceBidEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/auctions/{auctionId:guid}/bids", async (Guid auctionId,PlaceBidRequest request, PlaceBidHandler handler, CancellationToken cancellationtoken) =>
            {
                var commnad = new PlaceBidCommand(auctionId, request.BidderId, request.BidAmount);

                var result = await handler.Handle(commnad, cancellationtoken);


                if (result.IsFailure)
                {
                    // If the auction was not found, return a 404 Not Found response.
                    if (result.Error.Code is "Auction.NotFound" or "Bidder.NotFound")
                    {
                        return Results.NotFound(result.Error);
                    }

                    if (result.Error.Code is
                        "Auction.NotLive" or
                        "Auction.NotActive" or
                        "Auction.SellerCannotBid" or
                        "Bid.TooLow" or
                        "Bid.CannotPlace")
                    {
                        return Results.Conflict(result.Error);
                    }

                    // For other errors, return a 400 Bad Request response with the error details.
                    return Results.BadRequest(result.Error);
                }

                return Results.Created($"/api/auctions/{auctionId}/bids/{result.Value.BidId}",result.Value);
                
            })
             .WithName("PlaceBid")
            .WithTags("Auctions")
            .Produces<PlaceBidResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict); ;
        }
    }
}
