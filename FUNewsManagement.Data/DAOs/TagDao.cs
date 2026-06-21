using FUNewsManagement.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagement.Data.DAOs;

public sealed class TagDao
{
    private static readonly Lazy<TagDao> _instance = new(() => new TagDao());
    public static TagDao Instance => _instance.Value;

    private TagDao()
    {
    }

    public async Task<List<Tag>> GetAllAsync()
    {
        await using var db = DbContextFactorySingleton.Instance.CreateDbContext();
        return await db.Tags.AsNoTracking().OrderBy(t => t.TagID).ToListAsync();
    }
}

