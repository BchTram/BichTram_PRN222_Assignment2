using FUNewsManagement.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagement.Data.DAOs;

public sealed class NewsArticleDao
{
    private static readonly Lazy<NewsArticleDao> _instance = new(() => new NewsArticleDao());
    public static NewsArticleDao Instance => _instance.Value;

    private NewsArticleDao()
    {
    }

    public async Task<List<NewsArticle>> SearchAsync(string? keyword, bool? onlyActive = null)
    {
        await using var db = DbContextFactorySingleton.Instance.CreateDbContext();
        IQueryable<NewsArticle> q = db.NewsArticles
            .AsNoTracking()
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Include(n => n.NewsTags).ThenInclude(nt => nt.Tag);

        if (onlyActive is not null)
        {
            q = q.Where(n => n.NewsStatus == onlyActive.Value);
        }

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            keyword = keyword.Trim();
            q = q.Where(n =>
                (n.NewsTitle ?? "").Contains(keyword) ||
                n.Headline.Contains(keyword) ||
                (n.NewsContent ?? "").Contains(keyword));
        }

        return await q.OrderByDescending(n => n.CreatedDate).ToListAsync();
    }

    public async Task<List<NewsArticle>> GetByCreatorAsync(short creatorId)
    {
        await using var db = DbContextFactorySingleton.Instance.CreateDbContext();
        return await db.NewsArticles.AsNoTracking()
            .Where(n => n.CreatedByID == creatorId)
            .Include(n => n.Category)
            .Include(n => n.NewsTags).ThenInclude(nt => nt.Tag)
            .OrderByDescending(n => n.CreatedDate)
            .ToListAsync();
    }

    public async Task<NewsArticle?> GetByIdAsync(string id)
    {
        await using var db = DbContextFactorySingleton.Instance.CreateDbContext();
        return await db.NewsArticles.AsNoTracking()
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Include(n => n.NewsTags).ThenInclude(nt => nt.Tag)
            .FirstOrDefaultAsync(n => n.NewsArticleID == id);
    }

    public async Task CreateOrUpdateAsync(NewsArticle article, IReadOnlyCollection<int> tagIds)
    {
        await using var db = DbContextFactorySingleton.Instance.CreateDbContext();

        var existing = await db.NewsArticles
            .Include(n => n.NewsTags)
            .FirstOrDefaultAsync(n => n.NewsArticleID == article.NewsArticleID);

        if (existing is null)
        {
            db.NewsArticles.Add(article);
            existing = article;
        }
        else
        {
            db.Entry(existing).CurrentValues.SetValues(article);
        }

        // Sync tags
        var keep = new HashSet<int>(tagIds);
        existing.NewsTags.RemoveAll(nt => !keep.Contains(nt.TagID));
        foreach (var tagId in keep)
        {
            if (existing.NewsTags.All(nt => nt.TagID != tagId))
            {
                existing.NewsTags.Add(new NewsTag { NewsArticleID = existing.NewsArticleID, TagID = tagId });
            }
        }

        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        await using var db = DbContextFactorySingleton.Instance.CreateDbContext();
        // DB FK NewsTag -> NewsArticle does NOT cascade in the provided script,
        // so we must delete link rows first.
        var existing = await db.NewsArticles
            .Include(n => n.NewsTags)
            .FirstOrDefaultAsync(n => n.NewsArticleID == id);
        if (existing is null) return;

        if (existing.NewsTags.Count > 0)
        {
            db.NewsTags.RemoveRange(existing.NewsTags);
        }

        db.NewsArticles.Remove(existing);
        await db.SaveChangesAsync();
    }

    public async Task<List<NewsArticle>> ReportByCreatedDateAsync(DateTime start, DateTime end)
    {
        await using var db = DbContextFactorySingleton.Instance.CreateDbContext();
        return await db.NewsArticles.AsNoTracking()
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Where(n => n.CreatedDate >= start && n.CreatedDate <= end)
            .OrderByDescending(n => n.CreatedDate)
            .ToListAsync();
    }
}

internal static class ListExtensions
{
    public static void RemoveAll<T>(this ICollection<T> collection, Func<T, bool> predicate)
    {
        var toRemove = collection.Where(predicate).ToList();
        foreach (var item in toRemove) collection.Remove(item);
    }
}

