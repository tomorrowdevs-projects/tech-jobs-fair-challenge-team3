using System;
using System.Collections.Generic;

namespace RubricaTelefonicaAziendale.Entities;

public partial class Roles
{
    public string Id { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Users> Users { get; set; } = new List<Users>();
}
