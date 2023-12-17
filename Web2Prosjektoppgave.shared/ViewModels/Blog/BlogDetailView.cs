using Web2Prosjektoppgave.shared.ViewModels.BlogPost;

namespace Web2Prosjektoppgave.shared.ViewModels.Blog;

public class BlogDetailView
{
    public int Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string CreatedByUserName { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
    public string ModifiedByUserName { get; set; }
    public IList<BlogPostItemView> BlogPosts { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }
}