using FUNewsManagement.Data.DAOs;
using FUNewsManagement.Data.Models;
using FUNewsManagement.Repository.Interfaces;

namespace FUNewsManagement.Repository.Implementations;

public sealed class TagRepository : ITagRepository
{
    public Task<List<Tag>> GetAllAsync() => TagDao.Instance.GetAllAsync();
}

