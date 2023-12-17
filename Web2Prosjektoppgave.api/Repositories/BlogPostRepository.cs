using Microsoft.EntityFrameworkCore;
using Web2Prosjektoppgave.api.Data;
using Web2Prosjektoppgave.api.Models.Entities;
using Web2Prosjektoppgave.api.Models.Interfaces;

namespace Web2Prosjektoppgave.api.Repositories;

public class BlogPostRepository : IBlogPostRepository
{
    private readonly BlogDbContext _dbContext;

    public BlogPostRepository(BlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Insert(BlogPost blogPost)
    {
        _dbContext.BlogPosts.Add(blogPost);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<BlogPost?> GetById(int postId)
    {
        return await _dbContext.BlogPosts
            .Include(blogPost => blogPost.CreatedBy)
            .Include(blogPost => blogPost.ModifiedBy)
            .FirstOrDefaultAsync(blogPost => blogPost.Id == postId);
    }

    public async Task Update(BlogPost blogPost)
    {
        _dbContext.Entry(blogPost).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(BlogPost blogPost)
    {
        _dbContext.BlogPosts.Remove(blogPost);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IList<BlogPost>> GetAllByBlogId(int blogId)
    {
        return await _dbContext.BlogPosts
            .Include(blogPost => blogPost.CreatedBy)
            .Include(blogPost => blogPost.ModifiedBy)
            .Where(blogPost => blogPost.BlogId == blogId)
            .ToListAsync();
    }

    public async Task<IList<BlogPost>> GetAllByUserId(int userId)
    {
        return await _dbContext.BlogPosts
            .Include(blogPost => blogPost.CreatedBy)
            .Include(blogPost => blogPost.ModifiedBy)
            .Where(blogPost => blogPost.CreatedById == userId)
            .ToListAsync();
    }

    public async Task<IList<BlogPost>> Search(string phrase)
    {
        return await _dbContext.BlogPosts
            .Include(blogPost => blogPost.CreatedBy)
            .Include(blogPost => blogPost.ModifiedBy)
            .Where(blogPost => blogPost.BlogPostTags.Any(tag => tag.Name == phrase) || blogPost.CreatedBy.UserName.Contains(phrase))
            .ToListAsync();
    }
}