namespace RubricaTelefonicaAziendale.Entities;

public partial class Contacts
{
    public Guid Id { get; set; }

    public Guid ContactTypeId { get; set; }

    public string? Contact { get; set; }

    public virtual ContactTypes ContactType { get; set; } = null!;

    public virtual ICollection<People> Person { get; set; } = new List<People>();
}
