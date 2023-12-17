using System.ComponentModel.DataAnnotations;

namespace Web2Prosjektoppgave.shared.ViewModels.BlogPost;

public class BlogPostCreateForm
{
    public int BlogId { get; set; }

    [MaxLength(50, ErrorMessage = "Title cannot exceed 50 characters.")]
    [Required(ErrorMessage = "A title is required.")]
    public string Title { get; set; }
    [MaxLength(1000, ErrorMessage = "A Blog Post cannot exceed 1000 characters.")]
    public string Content { get; set; }
}