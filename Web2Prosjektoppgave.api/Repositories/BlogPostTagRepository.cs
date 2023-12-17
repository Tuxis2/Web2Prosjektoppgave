using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web2Prosjektoppgave.api.Data;
using Web2Prosjektoppgave.api.Models.Entities;
using Web2Prosjektoppgave.api.Models.Interfaces;

namespace Web2Prosjektoppgave.api.Repositories;

public class BlogPostTagRepository: IBlogPostTagRepository
{
    private readonly BlogDbContext _dbContext;

    public BlogPostTagRepository(BlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Insert(BlogPostTag tag)
    {
        _dbContext.BlogPostTags.Add(tag);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<BlogPostTag?> GetById(int tagId)
    {
        return await _dbContext.BlogPostTags
            .FirstOrDefaultAsync(tag => tag.Id == tagId);
    }

    public async Task<BlogPostTag?> GetByName(string name)
    {
        return await _dbContext.BlogPostTags
            .FirstOrDefaultAsync(tag => tag.Name == name);
    }

    public async Task Update(BlogPostTag tag)
    {
        _dbContext.Entry(tag).State = EntityState.Modified;

        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(BlogPostTag tag)
    {
        _dbContext.BlogPostTags.Remove(tag);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<IList<BlogPostTag>> GetAll()
    {
        return await _dbContext.BlogPostTags
            .ToListAsync();
    }

    public async Task<IList<BlogPostTag>> SearchBlogPostTags(string searchTerm)
    {
        return await _dbContext.BlogPostTags
            .Where(tag => tag.Name.Contains(searchTerm))
            .ToListAsync();
    }
}