using FUNewsManagement.Data.DAOs;
using FUNewsManagement.Data.Models;
using FUNewsManagement.Repository.Interfaces;


namespace FUNewsManagement.Repository.Implementations;

public sealed class AccountRepository : IAccountRepository
{
    public Task<List<SystemAccount>> SearchAsync(string? keyword) => SystemAccountDao.Instance.SearchAsync(keyword);

    public Task<SystemAccount?> GetByIdAsync(short id) => SystemAccountDao.Instance.GetByIdAsync(id);

    public Task<SystemAccount?> LoginStaffOrLecturerAsync(string email, string password) =>
        SystemAccountDao.Instance.GetByEmailPasswordAsync(email, password);

    public Task CreateAsync(SystemAccount account) => SystemAccountDao.Instance.CreateAsync(account);

    public Task UpdateAsync(SystemAccount account) => SystemAccountDao.Instance.UpdateAsync(account);

    public Task DeleteAsync(short id) => SystemAccountDao.Instance.DeleteAsync(id);
}

