using LiveBid.Tests.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Tests
{
    public sealed class DatabaseSmokeTests
    {
        [Fact]
        public async Task TestDatabase_ShouldBeAvailable()
        {
            await using var dbContext = TestDbContextFactory.Create();

            await dbContext.Database.EnsureCreatedAsync();

            var canConnect = await dbContext.Database.CanConnectAsync();

            Assert.True(canConnect);
        }


    }
}
