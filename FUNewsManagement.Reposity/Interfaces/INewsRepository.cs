using FUNewsManagement.Data.Models;

namespace FUNewsManagement.Repository.Interfaces;

public interface INewsRepository
{
    Task<List<NewsArticle>> SearchAsync(string? keyword, bool? onlyActive = null);
    Task<List<NewsArticle>> GetByCreatorAsync(short creatorId);
    Task<NewsArticle?> GetByIdAsync(string id);
    Task CreateOrUpdateAsync(NewsArticle article, IReadOnlyCollection<int> tagIds);
    Task DeleteAsync(string id);
    Task<List<NewsArticle>> ReportByCreatedDateAsync(DateTime start, DateTime end);
}

