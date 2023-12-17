using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Web2Prosjektoppgave.api.Data;
using Web2Prosjektoppgave.api.Models.Entities;
using Web2Prosjektoppgave.api.Models.Interfaces;

namespace Web2Prosjektoppgave.api.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly BlogDbContext _dbContext;

    public CommentRepository(BlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Insert(Comment comment)
    {
        _dbContext.Comments.Add(comment);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Comment?> GetById(int commentId)
    {
        return await _dbContext.Comments
            .Include(comment => comment.CreatedBy)
            .Include(comment => comment.ModifiedBy)
            .FirstOrDefaultAsync(comment => comment.Id == commentId);
    }

    public async Task Update(Comment comment)
    {
        _dbContext.Entry(comment).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(Comment comment)
    {
        _dbContext.Comments.Remove(comment);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IList<Comment>> GetAllByBlogPostId(int blogPostId)
    {
        return await _dbContext.Comments
            .Include(comment => comment.CreatedBy)
            .Include(comment => comment.ModifiedBy)
            .Where(comment => comment.BlogPostId == blogPostId)
            .ToListAsync();
    }

    public async Task<IList<Comment>> GetAllByUserId(int userId)
    {
        return await _dbContext.Comments
            .Include(comment => comment.CreatedBy)
            .Include(comment => comment.ModifiedBy)
            .Where(comment => comment.CreatedById == userId)
            .ToListAsync();
    }
}