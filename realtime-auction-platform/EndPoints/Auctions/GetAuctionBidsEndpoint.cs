using LiveBid.Application.Features.Auctions.GetAuctionBids;

namespace realtime_auction_platform.EndPoints.Auctions
{
    public static class GetAuctionBidsEndpoint
    {
        public static void MapGetAuctionBidsEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/auctions/{auctionId:guid}/bids", async (Guid auctionId, [AsParameters] GetAuctionBidRequest request, GetAuctionBidsHandler handler, CancellationToken cancellationToken) =>
            {
                var result = await handler.Handle(new GetAuctionBidsQuery(auctionId, request.Page, request.PageSize, request.SortDirection), cancellationToken);

                // Check if the result indicates a failure and handle accordingly.
                if (result.IsFailure)
                {
                    // If the auction was not found, return a 404 Not Found response.
                    if (result.Error.Code == "Auction.NotFound")
                    {
                        return Results.NotFound(result.Error);
                    }
                    // For other errors, return a 400 Bad Request response with the error details.
                    return Results.BadRequest(result.Error);
                }

               
                return Results.Ok(result.Value);
            }).WithName("GetAuctionBids")
                .WithTags("Auctions")
                .Produces<GetAuctionBidsResponse>(
                StatusCodes.Status200OK)
            .Produces(
                StatusCodes.Status400BadRequest)
            .Produces(
                StatusCodes.Status404NotFound);
        }
    }
}
