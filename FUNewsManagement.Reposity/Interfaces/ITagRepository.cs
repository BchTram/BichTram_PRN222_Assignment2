using FUNewsManagement.Data.Models;

namespace FUNewsManagement.Repository.Interfaces;

public interface ITagRepository
{
    Task<List<Tag>> GetAllAsync();
}

