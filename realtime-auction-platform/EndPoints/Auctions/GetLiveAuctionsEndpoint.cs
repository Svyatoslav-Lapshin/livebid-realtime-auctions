using LiveBid.Application.Features.Auctions.GetLiveAuctions;

namespace realtime_auction_platform.EndPoints.Auctions
{
    public static class GetLiveAuctionsEndpoint
    {
        public static void MapGetLiveAuctionsEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/auctions/live", async ([AsParameters] GetLiveAuctionsQuery query, GetLiveAuctionsHandler handler, CancellationToken cancellationToken) =>
            {
                var result = await handler.Handle(query, cancellationToken);
                if (result is null)
                {
                    return Results.Problem(
                        title: "Error retrieving live auctions",
                        detail: "GetLiveAuctionsHandler returned null",
                        statusCode: StatusCodes.Status500InternalServerError
                        );
                }

                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                if (result.Value is null)
                {
                    return Results.Problem(
                       title: "Unexpected error",
                       detail: "Successfully retrieved live auctions but with null value",
                       statusCode: StatusCodes.Status500InternalServerError
                       );
                }
                return Results.Ok(result.Value);
            }).WithName("GetLiveAuctions")
                .WithTags("Auctions");
        }

    }
}
