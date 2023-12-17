using Web2Prosjektoppgave.api.Models.Entities;

namespace Web2Prosjektoppgave.api.Models.Interfaces;

public interface IBlogRepository
{
    Task Insert(Blog blog);
    Task<Blog?> GetById(int blogId);
    Task Update(Blog blog);
    Task Delete(Blog blog);
    Task<IList<Blog>> GetAll();
    Task<IList<Blog>> GetAllByUserId(int userId);
    Task<IList<Blog>> GetMostRecent(int count);
}