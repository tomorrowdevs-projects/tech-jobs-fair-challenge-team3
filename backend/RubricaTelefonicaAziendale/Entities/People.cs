namespace RubricaTelefonicaAziendale.Entities;

public partial class People
{
    public Guid Id { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string? Picture { get; set; }

    public bool IsEmployee { get; set; }

    public bool IsCustomer { get; set; }

    public bool IsPartner { get; set; }

    public virtual ICollection<Contacts> Contact { get; set; } = new List<Contacts>();

    public virtual ICollection<Groups> Group { get; set; } = new List<Groups>();
}
