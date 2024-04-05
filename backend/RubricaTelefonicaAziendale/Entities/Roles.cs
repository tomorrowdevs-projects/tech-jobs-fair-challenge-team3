namespace RubricaTelefonicaAziendale.Entities;

public partial class Roles
{
    public Guid Id { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<Users> Users { get; set; } = new List<Users>();
}
