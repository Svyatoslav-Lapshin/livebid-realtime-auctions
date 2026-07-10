using LiveBid.Application.Features.Auctions.GetAuctionById;

namespace realtime_auction_platform.EndPoints.Auctions
{
    public static class GetAuctionByIdEndpoint
    {
        public static void MapGetAuctionByIdEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/auctions/{auctionId}", async (Guid auctionId,GetAuctionByIdHandler handler, CancellationToken cancellationToken) =>
            {
                var query = new GetAuctionByIdQuery(auctionId);

                var result = await handler.Handle(query, cancellationToken);

                if (result is null)
                {
                    return Results.Problem(
                        title: "Error retrieving auction",
                        detail: "GetAuctionByIdHandler returned null",
                        statusCode: StatusCodes.Status500InternalServerError
                        );
                }
                if (result.IsFailure)
                {
                    if (result.Error.Code=="Auction.NotFound")
                    {
                        return Results.NotFound(result.Error);
                    }
                
                    return Results.BadRequest(result.Error);
                }
              
                return Results.Ok(result.Value);
            });
        }


    }
}
