using System;
using System.Collections.Generic;

namespace RubricaTelefonicaAziendale.Entities;

public partial class People
{
    public string Id { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string? Picture { get; set; }

    public virtual ICollection<Contacts> Contact { get; set; } = new List<Contacts>();

    public virtual ICollection<Groups> Group { get; set; } = new List<Groups>();
}
