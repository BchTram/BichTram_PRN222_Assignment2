using Microsoft.EntityFrameworkCore;

namespace FUNewsManagement.Data;


public sealed class DbContextFactorySingleton
{
    private static readonly Lazy<DbContextFactorySingleton> _instance = new(() => new DbContextFactorySingleton());

    private DbContextOptions<FUNewsManagementDbContext>? _options;

    private DbContextFactorySingleton()
    {
    }

    public static DbContextFactorySingleton Instance => _instance.Value;

    public void Configure(string connectionString)
    {
        var builder = new DbContextOptionsBuilder<FUNewsManagementDbContext>();
        builder.UseSqlServer(connectionString);
        _options = builder.Options;
    }

    public FUNewsManagementDbContext CreateDbContext()
    {
        if (_options is null)
        {
            throw new InvalidOperationException("DbContextFactorySingleton is not configured. Call Configure(connectionString) at app startup.");
        }

        return new FUNewsManagementDbContext(_options);
    }
}

