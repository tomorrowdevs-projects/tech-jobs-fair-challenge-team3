using System;
using System.Collections.Generic;

namespace RubricaTelefonicaAziendale.Entities;

public partial class Contacts
{
    public string Id { get; set; } = null!;

    public string ContactTypeId { get; set; } = null!;

    public string? Contact { get; set; }

    public virtual ContactTypes ContactType { get; set; } = null!;

    public virtual ICollection<People> Person { get; set; } = new List<People>();
}
