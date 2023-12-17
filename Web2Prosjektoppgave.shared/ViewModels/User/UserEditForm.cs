using System.ComponentModel.DataAnnotations;

namespace Web2Prosjektoppgave.shared.ViewModels.User;

public class UserEditForm
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Email { get; set; }
}