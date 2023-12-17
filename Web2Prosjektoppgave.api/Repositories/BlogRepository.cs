using Microsoft.EntityFrameworkCore;
using Web2Prosjektoppgave.api.Data;
using Web2Prosjektoppgave.api.Models.Entities;
using Web2Prosjektoppgave.api.Models.Interfaces;

namespace Web2Prosjektoppgave.api.Repositories;

public class BlogRepository : IBlogRepository
{
    private readonly BlogDbContext _dbContext;

    public BlogRepository(BlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Insert(Blog blog)
    {
        _dbContext.Add(blog);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Blog?> GetById(int blogId)
    {
        return await _dbContext.Blogs
            .Include(blog => blog.CreatedBy)
            .Include(blog => blog.ModifiedBy)
            .FirstOrDefaultAsync(blog => blog.Id == blogId);
    }

    public async Task Update(Blog blog)
    {
        _dbContext.Entry(blog).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(Blog blog)
    {
        _dbContext.Blogs.Remove(blog);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IList<Blog>> GetAll()
    {
        return await _dbContext.Blogs
            .Include(blog => blog.CreatedBy)
            .Include(blog => blog.ModifiedBy)
            .ToListAsync();
    }

    public async Task<IList<Blog>> GetAllByUserId(int userId)
    {
        return await _dbContext.Blogs
            .Include(blog => blog.CreatedBy)
            .Include(blog => blog.ModifiedBy)
            .Where(blog => blog.CreatedById == userId)
            .ToListAsync();
    }

    public async Task<IList<Blog>> GetMostRecent(int count)
    {
        return await _dbContext.Blogs
            .Include(blog => blog.CreatedBy)
            .Include(blog => blog.ModifiedBy)
            .OrderByDescending(blog => blog.CreatedAt)
            .Take(count)
            .ToListAsync();
    }
}