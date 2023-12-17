namespace Web2Prosjektoppgave.api.Models.Entities;

public class User
{
    public int Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public int CreatedById { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
    public int ModifiedById { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string HashedPassword { get; set; }
    public string Salt { get; set; }
}