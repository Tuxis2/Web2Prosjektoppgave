namespace Web2Prosjektoppgave.api.Models.Entities;

public class BlogPostTag
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IList<BlogPost> BlogPosts { get; set; }
}