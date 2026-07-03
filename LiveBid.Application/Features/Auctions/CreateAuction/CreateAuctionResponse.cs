
namespace LiveBid.Application.Features.Auctions.CreateAuction;

public sealed class CreateAuctionResponse
{
    public Guid AuctionId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal StartPrice { get; init; }
    public decimal CurrentPrice { get; init; }
    public DateTimeOffset StartTime { get; init; }
    public DateTimeOffset EndTime { get; init; }
    public string Status { get; init; } = string.Empty;
}