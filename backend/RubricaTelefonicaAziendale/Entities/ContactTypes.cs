using System;
using System.Collections.Generic;

namespace RubricaTelefonicaAziendale.Entities;

public partial class ContactTypes
{
    public string Id { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? Validation { get; set; }

    public virtual ICollection<Contacts> Contacts { get; set; } = new List<Contacts>();
}
