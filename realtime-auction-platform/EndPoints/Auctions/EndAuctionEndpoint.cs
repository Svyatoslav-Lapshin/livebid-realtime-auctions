using LiveBid.Application.Features.Auctions.EndAuction;

namespace realtime_auction_platform.EndPoints.Auctions
{
    public static class EndAuctionEndpoint
    {
        public static void MapEndAuctionEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPatch("/api/auctions/{auctionId:guid}/end", async (Guid auctionId, EndAuctionHandler handler, CancellationToken cancellationToken) =>
            {
                var command = new EndAuctionCommand(auctionId);

                var result = await handler.Handle(command, cancellationToken);

                if (result.IsFailure)
                {
                    if (result.Error.Code == "Auction.NotFound")
                    {
                        return Results.NotFound(result.Error);
                    }
                    if (result.Error.Code == "Auction.CannotEnd")
                    {
                        return Results.Conflict(result.Error);
                    }
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);

            }).WithName("EndAuction")
            .WithTags("Auctions")
            .Produces<EndAuctionResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict);
        }


    }
}
