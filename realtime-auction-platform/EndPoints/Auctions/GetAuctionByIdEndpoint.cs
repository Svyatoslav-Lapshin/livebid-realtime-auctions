using LiveBid.Application.Features.Auctions.GetAuctionById;

namespace realtime_auction_platform.EndPoints.Auctions
{
    public static class GetAuctionByIdEndpoint
    {
        // This method maps the endpoint for retrieving an auction by its ID.
        public static void MapGetAuctionByIdEndpoint(this IEndpointRouteBuilder app)
        {
            // Define the GET endpoint for retrieving an auction by its ID.
            app.MapGet("/api/auctions/{auctionId:guid}", async (Guid auctionId, GetAuctionByIdHandler handler, CancellationToken cancellationToken) =>
            {
                // Create a query object with the provided auction ID.
                var query = new GetAuctionByIdQuery(auctionId);

                // Call the handler to process the query and retrieve the auction.
                var result = await handler.Handle(query, cancellationToken);

                // Check if the result is null, indicating an unexpected error.
                if (result is null)
                {
                    return Results.Problem(
                        title: "Error retrieving auction",
                        detail: "GetAuctionByIdHandler returned null",
                        statusCode: StatusCodes.Status500InternalServerError
                        );
                }
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
                // If the result is successful, return a 200 OK response with the auction data.
                return Results.Ok(result.Value);
            })
                .WithName("GetAuctionById")
                .WithTags("Auctions")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest); 
        }



    }
}
