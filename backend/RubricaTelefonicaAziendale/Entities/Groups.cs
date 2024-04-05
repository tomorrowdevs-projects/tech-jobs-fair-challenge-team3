namespace RubricaTelefonicaAziendale.Entities;

public partial class Groups
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<People> Person { get; set; } = new List<People>();
}
