using LiveBid.Application.Features.Auctions.StartAuction;


namespace realtime_auction_platform.EndPoints.Auctions
{
    public static class StartAuctionEndpoint
    {
        public static void MapStartAuctionEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPatch("/api/auctions/{auctionId:guid}/start", async (Guid auctionId, StartAuctionHandler handler, CancellationToken cancellationToken) =>
            {
                var command = new StartAuctionCommand(auctionId);

                var result = await handler.Handle(command, cancellationToken);

      
                if (result.IsFailure)
                {
                    if (result.Error.Code=="Auction.NotFound")
                    {
                        return Results.NotFound(result.Error);
                    }

                    if (result.Error.Code=="Auction.CannotStart")
                    {
                        return Results.Conflict(result.Error);
                    }

                    return Results.BadRequest(result.Error);

                }

                return Results.Ok(result.Value);

            }).WithName("StartAuction")
            .WithTags("Auctions")
            .Produces<StartAuctionResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict);

        }



    }
}
