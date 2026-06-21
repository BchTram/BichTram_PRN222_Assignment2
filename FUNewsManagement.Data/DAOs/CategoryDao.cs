using FUNewsManagement.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagement.Data.DAOs;

public sealed class CategoryDao
{
    private static readonly Lazy<CategoryDao> _instance = new(() => new CategoryDao());
    public static CategoryDao Instance => _instance.Value;

    private CategoryDao()
    {
    }

    public async Task<List<Category>> SearchAsync(string? keyword)
    {
        await using var db = DbContextFactorySingleton.Instance.CreateDbContext();
        IQueryable<Category> q = db.Categories.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            keyword = keyword.Trim();
            q = q.Where(c => c.CategoryName.Contains(keyword) || c.CategoryDesciption.Contains(keyword));
        }

        return await q.OrderBy(c => c.CategoryID).ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(short id)
    {
        await using var db = DbContextFactorySingleton.Instance.CreateDbContext();
        return await db.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.CategoryID == id);
    }

    public async Task<bool> HasNewsAsync(short categoryId)
    {
        await using var db = DbContextFactorySingleton.Instance.CreateDbContext();
        return await db.NewsArticles.AnyAsync(n => n.CategoryID == categoryId);
    }

    public async Task CreateAsync(Category category)
    {
        await using var db = DbContextFactorySingleton.Instance.CreateDbContext();
        db.Categories.Add(category);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Category category)
    {
        await using var db = DbContextFactorySingleton.Instance.CreateDbContext();
        db.Categories.Update(category);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(short id)
    {
        await using var db = DbContextFactorySingleton.Instance.CreateDbContext();
        var existing = await db.Categories.FirstOrDefaultAsync(x => x.CategoryID == id);
        if (existing is null) return;
        db.Categories.Remove(existing);
        await db.SaveChangesAsync();
    }
}

