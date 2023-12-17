namespace Web2Prosjektoppgave.shared.ViewModels.Comment;

public class CommentItemSignal
{
    public int Id { get; set; }
    public int BlogPostId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public string CreatedByUserName { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
    public string ModifiedByUserName { get; set; }
    public string Content { get; set; }
}