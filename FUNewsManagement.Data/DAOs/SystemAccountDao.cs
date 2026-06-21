using FUNewsManagement.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagement.Data.DAOs;

public sealed class SystemAccountDao
{
    private static readonly Lazy<SystemAccountDao> _instance = new(() => new SystemAccountDao());
    public static SystemAccountDao Instance => _instance.Value;

    private SystemAccountDao()
    {
    }

    public async Task<List<SystemAccount>> SearchAsync(string? keyword)
    {
        await using var db = DbContextFactorySingleton.Instance.CreateDbContext();
        IQueryable<SystemAccount> q = db.SystemAccounts.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            keyword = keyword.Trim();
            q = q.Where(a =>
                (a.AccountName ?? "").Contains(keyword) ||
                (a.AccountEmail ?? "").Contains(keyword));
        }

        return await q.OrderBy(a => a.AccountID).ToListAsync();
    }

    public async Task<SystemAccount?> GetByIdAsync(short id)
    {
        await using var db = DbContextFactorySingleton.Instance.CreateDbContext();
        return await db.SystemAccounts.AsNoTracking().FirstOrDefaultAsync(x => x.AccountID == id);
    }

    public async Task<SystemAccount?> GetByEmailPasswordAsync(string email, string password)
    {
        await using var db = DbContextFactorySingleton.Instance.CreateDbContext();
        return await db.SystemAccounts.AsNoTracking()
            .FirstOrDefaultAsync(x => x.AccountEmail == email && x.AccountPassword == password);
    }

    public async Task CreateAsync(SystemAccount account)
    {
        await using var db = DbContextFactorySingleton.Instance.CreateDbContext();
        db.SystemAccounts.Add(account);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(SystemAccount account)
    {
        await using var db = DbContextFactorySingleton.Instance.CreateDbContext();
        db.SystemAccounts.Update(account);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(short id)
    {
        await using var db = DbContextFactorySingleton.Instance.CreateDbContext();
        var existing = await db.SystemAccounts.FirstOrDefaultAsync(x => x.AccountID == id);
        if (existing is null) return;
        db.SystemAccounts.Remove(existing);
        await db.SaveChangesAsync();
    }
}

