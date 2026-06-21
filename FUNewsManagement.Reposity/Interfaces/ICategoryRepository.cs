using FUNewsManagement.Data.Models;

namespace FUNewsManagement.Repository.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> SearchAsync(string? keyword);
    Task<Category?> GetByIdAsync(short id);
    Task CreateAsync(Category category);
    Task UpdateAsync(Category category);
    Task<bool> CanDeleteAsync(short id);
    Task DeleteAsync(short id);
}

