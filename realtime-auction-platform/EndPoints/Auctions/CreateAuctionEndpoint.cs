using LiveBid.Application.Features.Auctions.CreateAuction;


namespace realtime_auction_platform.EndPoints.Auctions
{
    public static class CreateAuctionEndpoint
    {

        public static void MapCreateAuctionEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/auctions",async  (CreateAuctionCommand command, CreateAuctionHandler handler, CancellationToken cancellationToken) =>
            {
               var result = await handler.Handle(command, cancellationToken);

                if (result is null)
                {
                    return Results.Problem(
                        title: "Error creating auction",
                        detail: "CreateAuctionHandler returned null",
                        statusCode: StatusCodes.Status500InternalServerError
                        );
                }

                if (result.IsFailure)
                {
                    return Results.BadRequest( result.Error );
                }


                if (result.Value is null)
                {
                    return Results.Problem(
                       title: "Unexpected error",
                       detail: "Successfully created auction but with null value",
                       statusCode: StatusCodes.Status500InternalServerError
                       );
                }



                return Results.Created($"/api/auctions/{result.Value.AuctionId}", result.Value);
            });
        }



    }
}
