using Web2Prosjektoppgave.api.Models.Entities;

namespace Web2Prosjektoppgave.api.Models.Interfaces;

public interface IUserRepository
{
    Task Insert(User user);
    Task<User?> GetById(int userId);
    Task<User?> GetByUserNameOrEmail(string usernameOrEmail);
    Task Update(User user);
    Task Delete(User user);
    Task<IList<User>> GetAll();
}