using System.Security.Cryptography.X509Certificates;

public sealed record CreateAuctionCommand(
     Guid SellerId,
     string Title,
     string Description,
     decimal StartPrice,
     DateTimeOffset StartTime,
     DateTimeOffset EndTime
);