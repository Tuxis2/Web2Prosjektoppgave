namespace Web2Prosjektoppgave.api.Models.Entities;

public class BlogPost
{
    public int Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public int CreatedById { get; set; }
    public User CreatedBy { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
    public int ModifiedById { get; set; }
    public User ModifiedBy { get; set; }

    public string Title { get; set; }
    public string Content { get; set; }
    public Blog Blog { get; set; }
    public int BlogId { get; set; }

    public IList<BlogPostTag> BlogPostTags { get; set; }
}