using FUNewsManagement.Data.DAOs;
using FUNewsManagement.Data.Models;
using FUNewsManagement.Repository.Interfaces;

namespace FUNewsManagement.Repository.Implementations;

public sealed class CategoryRepository : ICategoryRepository
{
    public Task<List<Category>> SearchAsync(string? keyword) => CategoryDao.Instance.SearchAsync(keyword);

    public Task<Category?> GetByIdAsync(short id) => CategoryDao.Instance.GetByIdAsync(id);

    public Task CreateAsync(Category category) => CategoryDao.Instance.CreateAsync(category);

    public Task UpdateAsync(Category category) => CategoryDao.Instance.UpdateAsync(category);

    public async Task<bool> CanDeleteAsync(short id)
    {
        var hasNews = await CategoryDao.Instance.HasNewsAsync(id);
        return !hasNews;
    }

    public Task DeleteAsync(short id) => CategoryDao.Instance.DeleteAsync(id);
}

