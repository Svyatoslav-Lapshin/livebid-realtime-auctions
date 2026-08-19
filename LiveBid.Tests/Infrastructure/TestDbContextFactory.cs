using LiveBid.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Tests.Infrastructure
{
    public static class TestDbContextFactory
    {
        public static LiveBidDbContext Create()
        {
            var connectionString = Environment.GetEnvironmentVariable(
                "LIVEBID_TEST_CONNECTION_STRING");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Environment variable LIVEBID_TEST_CONNECTION_STRING is not configured.");
            }

            var options = new DbContextOptionsBuilder<LiveBidDbContext>()
                .UseNpgsql(connectionString)
                .Options;

            return new LiveBidDbContext(options);
        }


    }
}
