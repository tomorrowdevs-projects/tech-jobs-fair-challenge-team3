namespace RubricaTelefonicaAziendale.Entities;

public partial class ContactTypes
{
    public Guid Id { get; set; }

    public string Type { get; set; } = null!;

    public string? Validation { get; set; }

    public virtual ICollection<Contacts> Contacts { get; set; } = new List<Contacts>();
}
