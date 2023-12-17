using Microsoft.EntityFrameworkCore;
using Web2Prosjektoppgave.api.Data;
using Web2Prosjektoppgave.api.Models.Entities;
using Web2Prosjektoppgave.api.Models.Interfaces;

namespace Web2Prosjektoppgave.api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly BlogDbContext _dbContext;

    public UserRepository(BlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Insert(User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<User?> GetById(int userId)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(user => user.Id == userId);
    }

    public async Task<User?> GetByUserNameOrEmail(string usernameOrEmail)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(user => user.UserName == usernameOrEmail || user.Email == usernameOrEmail);
    }

    public async Task Update(User user)
    {
        _dbContext.Entry(user).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(User user)
    {
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IList<User>> GetAll()
    {
        return await _dbContext.Users.ToListAsync();
    }
}