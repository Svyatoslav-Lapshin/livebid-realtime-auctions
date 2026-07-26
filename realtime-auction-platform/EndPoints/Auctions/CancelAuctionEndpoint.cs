using LiveBid.Application.Features.Auctions.CancelAuction;

namespace realtime_auction_platform.EndPoints.Auctions
{
    public static class CancelAuctionEndpoint
    {

        public static void MapCancelAuctionEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPatch("/api/auctions/{auctionId:guid}/cancel", async (Guid auctionId, CancelAuctionHandler handler, CancellationToken cancellationToken) =>
            {
                var command = new CancelAuctionCommand(auctionId);

                var result = await handler.Handle(command, cancellationToken);

               
                if (result.IsFailure)
                {
                    // If the auction was not found, return a 404 Not Found response.
                    if (result.Error.Code == "Auction.NotFound")
                    {
                        return Results.NotFound(result.Error);
                    }
                    if (result.Error.Code == "Auction.CannotCancel")
                    {
                        return Results.Conflict(result.Error);
                    }
                    // For other errors, return a 400 Bad Request response with the error details.
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);
            })
                .WithName("CancelAuction")
                .WithTags("Auctions")
                .Produces<CancelAuctionResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status409Conflict)
                .Produces(StatusCodes.Status400BadRequest);
        }
    }
}
