using System;
using System.Collections.Generic;

namespace RubricaTelefonicaAziendale.Entities;

public partial class Users
{
    public string Id { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public string? Picture { get; set; }

    public string RoleId { get; set; } = null!;

    public virtual Roles Role { get; set; } = null!;
}
