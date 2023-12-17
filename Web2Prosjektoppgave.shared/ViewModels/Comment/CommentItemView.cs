using Web2Prosjektoppgave.shared.ViewModels.BlogPost;

namespace Web2Prosjektoppgave.shared.ViewModels.Comment;

public class CommentItemView
{
    public int Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string CreatedByUserName { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
    public string ModifiedByUserName { get; set; }

    public string Content { get; set; }
}