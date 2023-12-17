using Web2Prosjektoppgave.shared.ViewModels.BlogPost;
using System.ComponentModel.DataAnnotations;

namespace Web2Prosjektoppgave.shared.ViewModels.Comment;

public class CommentEditForm
{
    public int Id { get; set; }
    public int BlogId { get; set; }
    public int BlogPostId { get; set; }
    [MaxLength(500, ErrorMessage = "A Comment cannot exceed 500 characters.")]
    [Required(ErrorMessage = "Cannot be blank, a comment is required.")]
    public string Content { get; set; }
    public BlogPostItemView? BlogPost { get; set; }
}