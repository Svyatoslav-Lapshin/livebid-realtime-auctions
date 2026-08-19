using LiveBid.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Tests.Infrastructure
{
    public static class TestDatabase
    {
        public static async Task InitializeAsync()
        {
            await using var dbContext = TestDbContextFactory.Create();

            await dbContext.Database.EnsureCreatedAsync();
        }

        public static async Task ResetAsync()
        {
            await using var dbContext = TestDbContextFactory.Create();

            var databaseName = dbContext.Database.GetDbConnection().Database;

            if (!string.Equals(
                    databaseName,
                    "livebid_test",
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    $"Refusing to clear database '{databaseName}'.");
            }

            await dbContext.Database.ExecuteSqlRawAsync("""
            TRUNCATE TABLE bids, auctions, users
            RESTART IDENTITY CASCADE;
            """);
        }


    }
}
