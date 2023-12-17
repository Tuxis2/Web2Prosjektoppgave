using Web2Prosjektoppgave.api.Models.Entities;

namespace Web2Prosjektoppgave.api.Models.Interfaces;

public interface ICommentRepository
{
    Task Insert(Comment comment);
    Task<Comment?> GetById(int commentId);
    Task Update(Comment comment);
    Task Delete(Comment comment);
    Task<IList<Comment>> GetAllByBlogPostId(int blogPostId);
    Task<IList<Comment>> GetAllByUserId(int userId);
}