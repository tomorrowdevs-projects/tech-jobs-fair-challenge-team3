namespace RubricaTelefonicaAziendale.Entities;

public partial class Users
{
    public Guid Id { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public string? Picture { get; set; }

    public Guid RoleId { get; set; }

    public virtual Roles Role { get; set; } = null!;
}
