using System.ComponentModel.DataAnnotations;

namespace Web2Prosjektoppgave.shared.ViewModels.Blog;

public class BlogCreateForm
{
    [MaxLength(50, ErrorMessage = "Title cannot exceed 50 characters.")]
    [Required(ErrorMessage = "A title is required.")]
    public string Title { get; set; }
    [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string Description { get; set; }
}