using LiveBid.Application.Features.Auctions.UpdateAuction;

namespace realtime_auction_platform.EndPoints.Auctions
{
    public static class UpdateAuctionEndpoint
    {

        public static void MapUpdateAuctionEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPut("/api/auctions/{auctionId:guid}", async (Guid auctionId, UpdateAuctionRequest request, UpdateAuctionHandler handler, CancellationToken cancellationToken) =>
            {
                var command = new UpdateAuctionCommand(auctionId, request.Title, request.Description, request.StartPrice, request.StartTime, request.EndTime);

                var result = await handler.Handle(command, cancellationToken);

                if (result is null)
                {
                    return Results.Problem(
                        title: "Error updating auction",
                        detail: "UpdateAuctionHandler returned null",
                        statusCode: StatusCodes.Status500InternalServerError
                        );
                }

                if (result.IsFailure)
                {
                    // If the auction was not found, return a 404 Not Found response.
                    if (result.Error.Code == "Auction.NotFound")
                    {
                        return Results.NotFound(result.Error);
                    }

                    if (result.Error.Code == "Auction.CannotUpdate")
                    {
                        return Results.Conflict(result.Error);
                    }

                    // For other errors, return a 400 Bad Request response with the error details.
                    return Results.BadRequest(result.Error);
                }
                return Results.Ok(result.Value);
            })
            .WithName("UpdateAuction")
            .WithTags("Auctions")
            .Produces<UpdateAuctionResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict);
        }
    }
}
