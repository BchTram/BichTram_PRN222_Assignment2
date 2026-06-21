using FUNewsManagement.Data.DAOs;
using FUNewsManagement.Data.Models;
using FUNewsManagement.Repository.Interfaces;


namespace FUNewsManagement.Repository.Implementations;

public sealed class NewsRepository : INewsRepository
{
    public Task<List<NewsArticle>> SearchAsync(string? keyword, bool? onlyActive = null) =>
        NewsArticleDao.Instance.SearchAsync(keyword, onlyActive);

    public Task<List<NewsArticle>> GetByCreatorAsync(short creatorId) =>
        NewsArticleDao.Instance.GetByCreatorAsync(creatorId);

    public Task<NewsArticle?> GetByIdAsync(string id) => NewsArticleDao.Instance.GetByIdAsync(id);

    public Task CreateOrUpdateAsync(NewsArticle article, IReadOnlyCollection<int> tagIds) =>
        NewsArticleDao.Instance.CreateOrUpdateAsync(article, tagIds);

    public Task DeleteAsync(string id) => NewsArticleDao.Instance.DeleteAsync(id);

    public Task<List<NewsArticle>> ReportByCreatedDateAsync(DateTime start, DateTime end) =>
        NewsArticleDao.Instance.ReportByCreatedDateAsync(start, end);
}

