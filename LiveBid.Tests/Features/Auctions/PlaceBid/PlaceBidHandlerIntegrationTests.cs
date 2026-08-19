using LiveBid.Application.Features.Auctions.PlaceBid;
using LiveBid.Domain.Auctions;
using LiveBid.Domain.Users;
using LiveBid.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Tests.Features.Auctions.PlaceBid
{
    public sealed class PlaceBidHandlerIntegrationTests : IntegrationTestBase
    {

        [Fact]
        public async Task Handle_WhenBidIsValid_ShouldReturnSuccess()
        {

            await using var dbContext = TestDbContextFactory.Create();
            //Arrange
            var seller = new User
            {

                DisplayName = "Test Seller",
                Email = "test@example.com"

            };
            var bidder = new User
            {
                DisplayName = "Test Bidder",
                Email = "testBidder@example.com"
            };
            dbContext.Users.AddRange(seller, bidder);
            await dbContext.SaveChangesAsync();

            var now = DateTimeOffset.UtcNow;
            var scheduleTime = now.AddMinutes(-2);
            var startTime = now.AddMinutes(-1);
            var endTime = now.AddMinutes(10);

            var auction = new Auction(seller.Id, "Test Auction", "Test Description", 100m, startTime, endTime);

            var wasScheduled = auction.Schedule(scheduleTime);
            Assert.True(wasScheduled);

            var wasStarted = auction.Start(now);

            Assert.True(wasStarted);
            Assert.Equal(AuctionStatus.Live, auction.Status);

            await dbContext.AddAuctionAsync(auction, CancellationToken.None);
            await dbContext.SaveChangesAsync();

            var placeBidCommand = new PlaceBidCommand(auction.Id, bidder.Id, 150m);

            var validator = new PlaceBidValidator();
            var handler = new PlaceBidHandler(dbContext, validator);

            //Act
            var result = await handler.Handle(placeBidCommand, CancellationToken.None);

            //Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_WhenBidIsValid_ShouldCreateBid()
        {
            await using var dbContext = TestDbContextFactory.Create();
            //Arrange
            var seller = new User
            {

                DisplayName = "Test Seller",
                Email = "test@example.com"

            };
            var bidder = new User
            {
                DisplayName = "Test Bidder",
                Email = "testBidder@example.com"
            };
            dbContext.Users.AddRange(seller, bidder);
            await dbContext.SaveChangesAsync();

            var now = DateTimeOffset.UtcNow;
            var scheduleTime = now.AddMinutes(-2);
            var startTime = now.AddMinutes(-1);
            var endTime = now.AddMinutes(10);

            var auction = new Auction(seller.Id, "Test Auction", "Test Description", 100m, startTime, endTime);

            var wasScheduled = auction.Schedule(scheduleTime);
            Assert.True(wasScheduled);

            var wasStarted = auction.Start(now);

            Assert.True(wasStarted);
            Assert.Equal(AuctionStatus.Live, auction.Status);

            await dbContext.AddAuctionAsync(auction, CancellationToken.None);
            await dbContext.SaveChangesAsync();

            var placeBidCommand = new PlaceBidCommand(auction.Id, bidder.Id, 150m);

            var validator = new PlaceBidValidator();
            var handler = new PlaceBidHandler(dbContext, validator);

            //Act
            var result = await handler.Handle(placeBidCommand, CancellationToken.None);

            //Assert
            Assert.True(result.IsSuccess);
            await using var verificationContext = TestDbContextFactory.Create();
            var savedBid = await verificationContext.Bids.AsNoTracking().SingleAsync(b => b.AuctionId == auction.Id);

            Assert.Equal(150m, savedBid.Amount);
            Assert.Equal(bidder.Id, savedBid.BidderId);
            Assert.Equal(auction.Id, savedBid.AuctionId);

        }

        [Fact]
        public async Task Handle_WhenBidIsValid_ShouldUpdateAuctionCurrentPrice()
        {
            await using var dbContext = TestDbContextFactory.Create();
            //Arrange
            var seller = new User
            {

                DisplayName = "Test Seller",
                Email = "test@example.com"

            };
            var bidder = new User
            {
                DisplayName = "Test Bidder",
                Email = "testBidder@example.com"
            };
            dbContext.Users.AddRange(seller, bidder);
            await dbContext.SaveChangesAsync();

            var now = DateTimeOffset.UtcNow;
            var scheduleTime = now.AddMinutes(-2);
            var startTime = now.AddMinutes(-1);
            var endTime = now.AddMinutes(10);

            var auction = new Auction(seller.Id, "Test Auction", "Test Description", 100m, startTime, endTime);

            var wasScheduled = auction.Schedule(scheduleTime);
            Assert.True(wasScheduled);

            var wasStarted = auction.Start(now);

            Assert.True(wasStarted);
            Assert.Equal(AuctionStatus.Live, auction.Status);

            await dbContext.AddAuctionAsync(auction, CancellationToken.None);
            await dbContext.SaveChangesAsync();

            var placeBidCommand = new PlaceBidCommand(auction.Id, bidder.Id, 150m);

            var validator = new PlaceBidValidator();
            var handler = new PlaceBidHandler(dbContext, validator);

            //Act
            var result = await handler.Handle(placeBidCommand, CancellationToken.None);

            //Assert
            Assert.True(result.IsSuccess);
            await using var verificationContext = TestDbContextFactory.Create();
            var savedAuction = await verificationContext.Auctions.AsNoTracking().SingleAsync(a => a.Id == auction.Id);

            Assert.Equal(150m, savedAuction.CurrentPrice);
        }

        [Fact]
        public async Task Handle_WhenBidIsTooLow_ShouldReturnFailure()
        {
            await using var dbContext = TestDbContextFactory.Create();
            //Arrange
            var seller = new User
            {
                DisplayName = "Test Seller",
                Email = "test@example.com"

            };
            var bidder = new User
            {
                DisplayName = "Test Bidder",
                Email = "testBidder@example.com"
            };
            dbContext.Users.AddRange(seller, bidder);
            await dbContext.SaveChangesAsync();

            var now = DateTimeOffset.UtcNow;
            var scheduleTime = now.AddMinutes(-2);
            var startTime = now.AddMinutes(-1);
            var endTime = now.AddMinutes(10);

            var auction = new Auction(seller.Id, "Test Auction", "Test Description", 100m, startTime, endTime);

            var wasScheduled = auction.Schedule(scheduleTime);
            Assert.True(wasScheduled);

            var wasStarted = auction.Start(now);

            Assert.True(wasStarted);
            Assert.Equal(AuctionStatus.Live, auction.Status);

            await dbContext.AddAuctionAsync(auction, CancellationToken.None);
            await dbContext.SaveChangesAsync();

            var placeBidCommand = new PlaceBidCommand(auction.Id, bidder.Id, 100m);

            var validator = new PlaceBidValidator();
            var handler = new PlaceBidHandler(dbContext, validator);

            var result = await handler.Handle(placeBidCommand, CancellationToken.None);

            //Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Bid.TooLow", result.Error.Code);

        }


        [Fact]
        public async Task Handle_WhenBidIsTooLow_ShouldNotCreateBid()
        {
            await using var dbContext = TestDbContextFactory.Create();
            //Arrange
            var seller = new User
            {
                DisplayName = "Test Seller",
                Email = "test@example.com"
            };
            var bidder = new User
            {
                DisplayName = "Test Bidder",
                Email = "testBidder@example.com"
            };
            dbContext.Users.AddRange(seller, bidder);
            await dbContext.SaveChangesAsync();

            var now = DateTimeOffset.UtcNow;
            var scheduleTime = now.AddMinutes(-2);
            var startTime = now.AddMinutes(-1);
            var endTime = now.AddMinutes(10);

            var auction = new Auction(seller.Id, "Test Auction", "Test Description", 100m, startTime, endTime);

            var wasScheduled = auction.Schedule(scheduleTime);
            Assert.True(wasScheduled);

            var wasStarted = auction.Start(now);

            Assert.True(wasStarted);
            Assert.Equal(AuctionStatus.Live, auction.Status);

            await dbContext.AddAuctionAsync(auction, CancellationToken.None);
            await dbContext.SaveChangesAsync();

            var placeBidCommand = new PlaceBidCommand(auction.Id, bidder.Id, 50m);

            var validator = new PlaceBidValidator();
            var handler = new PlaceBidHandler(dbContext, validator);

            var result = await handler.Handle(placeBidCommand, CancellationToken.None);

            Assert.True(result.IsFailure);
            Assert.Equal("Bid.TooLow", result.Error.Code);

            await using var verificationContext = TestDbContextFactory.Create();

            var bidExists = await verificationContext.Bids.AsNoTracking().AnyAsync(b => b.AuctionId == auction.Id);
            Assert.False(bidExists);

        }

        [Fact]
        public async Task Handle_WhenBidIsTooLow_ShouldNotUpdateCurrentPrice()
        {

            await using var dbContext = TestDbContextFactory.Create();
            //Arrange
            var seller = new User
            {
                DisplayName = "Test Seller",
                Email = "test@example.com"
            };
            var bidder = new User
            {
                DisplayName = "Test Bidder",
                Email = "testBidder@example.com"
            };
            dbContext.Users.AddRange(seller, bidder);
            await dbContext.SaveChangesAsync();

            var now = DateTimeOffset.UtcNow;
            var scheduleTime = now.AddMinutes(-2);
            var startTime = now.AddMinutes(-1);
            var endTime = now.AddMinutes(10);

            var auction = new Auction(seller.Id, "Test Auction", "Test Description", 100m, startTime, endTime);

            var wasScheduled = auction.Schedule(scheduleTime);
            Assert.True(wasScheduled);

            var wasStarted = auction.Start(now);

            Assert.True(wasStarted);
            Assert.Equal(AuctionStatus.Live, auction.Status);

            await dbContext.AddAuctionAsync(auction, CancellationToken.None);
            await dbContext.SaveChangesAsync();

            var placeBidCommand = new PlaceBidCommand(auction.Id, bidder.Id, 50m);

            var validator = new PlaceBidValidator();
            var handler = new PlaceBidHandler(dbContext, validator);

            var result = await handler.Handle(placeBidCommand, CancellationToken.None);

            Assert.True(result.IsFailure);
            Assert.Equal("Bid.TooLow", result.Error.Code);

            await using var verificationContext = TestDbContextFactory.Create();



            var savedAuction = await verificationContext.Auctions.AsNoTracking().SingleAsync(a => a.Id == auction.Id);
            Assert.Equal(100m, savedAuction.CurrentPrice);


        }

        [Fact]
        public async Task Handle_WhenSellerPlacesBid_ShouldReturnFailure()
        {

            await using var dbContext = TestDbContextFactory.Create();
            //Arrange
            var seller = new User
            {
                DisplayName = "Test Seller",
                Email = "test@example.com"
            };

            dbContext.Users.Add(seller);
            await dbContext.SaveChangesAsync();

            var now = DateTimeOffset.UtcNow;
            var scheduleTime = now.AddMinutes(-2);
            var startTime = now.AddMinutes(-1);
            var endTime = now.AddMinutes(10);

            var auction = new Auction(seller.Id, "Test Auction", "Test Description", 100m, startTime, endTime);

            var wasScheduled = auction.Schedule(scheduleTime);
            Assert.True(wasScheduled);

            var wasStarted = auction.Start(now);

            Assert.True(wasStarted);
            Assert.Equal(AuctionStatus.Live, auction.Status);

            await dbContext.AddAuctionAsync(auction, CancellationToken.None);
            await dbContext.SaveChangesAsync();

            var placeBidCommand = new PlaceBidCommand(auction.Id, seller.Id, 150m);

            var validator = new PlaceBidValidator();
            var handler = new PlaceBidHandler(dbContext, validator);

            var result = await handler.Handle(placeBidCommand, CancellationToken.None);

            Assert.True(result.IsFailure);
            Assert.Equal("Auction.SellerCannotBid", result.Error.Code);


        }


        // NOT FOUND

        [Fact]
        public async Task Handle_WhenBidderDoesNotExist_ShouldReturnNotFound()
        {

            await using var dbContext = TestDbContextFactory.Create();
            //Arrange
            var seller = new User
            {
                DisplayName = "Test Seller",
                Email = "test@example.com"
            };

            var nonExistentBidderId = Guid.NewGuid();
            dbContext.Users.Add(seller);
            await dbContext.SaveChangesAsync();

            var now = DateTimeOffset.UtcNow;
            var scheduleTime = now.AddMinutes(-2);
            var startTime = now.AddMinutes(-1);
            var endTime = now.AddMinutes(10);

            var auction = new Auction(seller.Id, "Test Auction", "Test Description", 100m, startTime, endTime);

            var wasScheduled = auction.Schedule(scheduleTime);
            Assert.True(wasScheduled);

            var wasStarted = auction.Start(now);

            Assert.True(wasStarted);
            Assert.Equal(AuctionStatus.Live, auction.Status);

            await dbContext.AddAuctionAsync(auction, CancellationToken.None);
            await dbContext.SaveChangesAsync();

            var placeBidCommand = new PlaceBidCommand(auction.Id, nonExistentBidderId, 150m);

            var validator = new PlaceBidValidator();
            var handler = new PlaceBidHandler(dbContext, validator);

            var result = await handler.Handle(placeBidCommand, CancellationToken.None);

            Assert.True(result.IsFailure);
            Assert.Equal("Bidder.NotFound", result.Error.Code);


        }

        [Fact]
        public async Task Handle_WhenAuctionDoesNotExist_ShouldReturnNotFound()
        {
            await using var dbContext = TestDbContextFactory.Create();
            //Arrange

            var bidder = new User
            {
                DisplayName = "Test Bidder",
                Email = "testBidder@example.com"
            };
            dbContext.Users.Add(bidder);
            await dbContext.SaveChangesAsync();

            var nonExistentAuctionId = Guid.NewGuid();

            var placeBidCommand = new PlaceBidCommand(nonExistentAuctionId, bidder.Id, 50m);

            var validator = new PlaceBidValidator();
            var handler = new PlaceBidHandler(dbContext, validator);

            var result = await handler.Handle(placeBidCommand, CancellationToken.None);

            Assert.True(result.IsFailure);
            Assert.Equal("Auction.NotFound", result.Error.Code);





        }


        // AUCTION STATE

        [Fact]
        public async Task Handle_WhenAuctionIsScheduled_ShouldReturnFailure()
        {
            await using var dbContext = TestDbContextFactory.Create();
            //Arrange
            var seller = new User
            {
                DisplayName = "Test Seller",
                Email = "test@example.com"
            };
            var bidder = new User
            {
                DisplayName = "Test Bidder",
                Email = "testBidder@example.com"
            };
            dbContext.Users.AddRange(seller, bidder);
            await dbContext.SaveChangesAsync();

            var now = DateTimeOffset.UtcNow;
            var startTime = now.AddMinutes(10);
            var endTime = now.AddMinutes(30);

            var auction = new Auction(seller.Id, "Test Auction", "Test Description", 100m, startTime, endTime);

            var wasScheduled = auction.Schedule(now);
            Assert.True(wasScheduled);

            Assert.Equal(AuctionStatus.Scheduled, auction.Status);



        }

        [Fact]
        public async Task Handle_WhenAuctionHasEnded_ShouldReturnFailure()
        {

            await using var dbContext = TestDbContextFactory.Create();
            //Arrange
            var seller = new User
            {
                DisplayName = "Test Seller",
                Email = "test@example.com"
            };
            var bidder = new User
            {
                DisplayName = "Test Bidder",
                Email = "testBidder@example.com"
            };
            dbContext.Users.AddRange(seller, bidder);
            await dbContext.SaveChangesAsync();

            var now = DateTimeOffset.UtcNow;
            var scheduleTime = now.AddMinutes(-3);
            var startTime = now.AddMinutes(-2);
            var endTime = now.AddMinutes(-1);

            var auction = new Auction(seller.Id, "Test Auction", "Test Description", 100m, startTime, endTime);

            var wasScheduled = auction.Schedule(scheduleTime);
            Assert.True(wasScheduled);

            var wasStarted = auction.Start(startTime.AddSeconds(10));

            Assert.True(wasStarted);
            Assert.Equal(AuctionStatus.Live, auction.Status);

            var wasEnded = auction.End(now);
            Assert.True(wasEnded);
            Assert.Equal(AuctionStatus.Ended, auction.Status);

            await dbContext.AddAuctionAsync(
                 auction,
                 CancellationToken.None);

            await dbContext.SaveChangesAsync();

            var command = new PlaceBidCommand(
                auction.Id,
                bidder.Id,
                150m);

            var validator = new PlaceBidValidator();
            var handler = new PlaceBidHandler(dbContext, validator);

            // Act
            var result = await handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Auction.Ended", result.Error.Code);

        }

       
    }
}
