using FUNewsManagement.Data.Models;

namespace FUNewsManagement.Repository.Interfaces;

public interface IAccountRepository
{
    Task<List<SystemAccount>> SearchAsync(string? keyword);
    Task<SystemAccount?> GetByIdAsync(short id);
    Task<SystemAccount?> LoginStaffOrLecturerAsync(string email, string password);
    Task CreateAsync(SystemAccount account);
    Task UpdateAsync(SystemAccount account);
    Task DeleteAsync(short id);
}

