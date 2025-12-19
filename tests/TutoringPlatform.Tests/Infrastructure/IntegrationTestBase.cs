using TutoringPlatform.Models;

namespace TutoringPlatform.Tests.Infrastructure;

public abstract class IntegrationTestBase : IDisposable
{
    protected readonly TutoringDbContext Context;
    private bool _disposed = false;

    protected IntegrationTestBase()
    {
        Context = TestDbContextFactory.CreateInMemoryContext();
    }

    protected async Task SeedDatabaseAsync()
    {
        await TestDbContextFactory.SeedTestDataAsync(Context);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                Context?.Database.EnsureDeleted();
                Context?.Dispose();
            }
            _disposed = true;
        }
    }
}
