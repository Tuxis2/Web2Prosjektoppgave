using System.ComponentModel.DataAnnotations;

namespace Web2Prosjektoppgave.shared.ViewModels.User;

public class UserCreateForm
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}