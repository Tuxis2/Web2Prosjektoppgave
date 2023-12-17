using Web2Prosjektoppgave.shared.ViewModels.Comment;

namespace Web2Prosjektoppgave.shared.ViewModels.BlogPost;

public class BlogPostItemView
{
    public int Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string CreatedByUserName { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
    public string ModifiedByUserName { get; set; }
    public IList<CommentItemView> BlogComments { get; set; }

    public string Title { get; set; }
    public string Content { get; set; }
}