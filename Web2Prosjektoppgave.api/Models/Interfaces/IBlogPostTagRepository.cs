using Microsoft.AspNetCore.Mvc;
using Web2Prosjektoppgave.api.Models.Entities;

namespace Web2Prosjektoppgave.api.Models.Interfaces;

public interface IBlogPostTagRepository
{
    Task Insert(BlogPostTag tag);
    Task<BlogPostTag?> GetById(int tagId);
    Task<BlogPostTag?> GetByName(string name);
    Task Update(BlogPostTag tag);
    Task Delete(BlogPostTag tag);

    Task<IList<BlogPostTag>> GetAll();
    Task<IList<BlogPostTag>> SearchBlogPostTags(string searchTerm);
}