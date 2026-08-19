using LiveBid.Tests.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Tests
{
    public abstract class IntegrationTestBase:IAsyncLifetime
    {
        public async Task InitializeAsync()
        {
            await TestDatabase.InitializeAsync();
            await TestDatabase.ResetAsync();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
