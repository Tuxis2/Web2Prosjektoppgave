namespace Web2Prosjektoppgave.api.Models.Entities;

public class Blog
{
    public int Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public int CreatedById { get; set; }
    public User CreatedBy { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
    public int ModifiedById { get; set; }
    public User ModifiedBy { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }
}