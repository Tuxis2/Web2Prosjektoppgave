using Web2Prosjektoppgave.api.Models.Entities;

namespace Web2Prosjektoppgave.api.Models.Interfaces;

public interface IBlogPostRepository
{
    Task Insert(BlogPost blogPost);
    Task<BlogPost?> GetById(int postId);
    Task Update(BlogPost blogPost);
    Task Delete(BlogPost blogPost);
    Task<IList<BlogPost>> GetAllByBlogId(int blogId);
    Task<IList<BlogPost>> GetAllByUserId(int userId);
    Task<IList<BlogPost>> Search(string phrase);
}