using System.Security.Cryptography.X509Certificates;

namespace LiveBid.Application.Features.Auctions.CreateAuction
{
    public sealed record CreateAuctionCommand(
         Guid SellerId,
         string Title,
         string Description,
         decimal StartPrice,
         DateTimeOffset StartTime,
         DateTimeOffset EndTime
    );
}