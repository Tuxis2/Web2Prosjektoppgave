namespace Web2Prosjektoppgave.shared.ViewModels.Blog;

public class BlogItemView
{
    public int Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string CreatedByUserName { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }
}