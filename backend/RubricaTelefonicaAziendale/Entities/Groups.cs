using System;
using System.Collections.Generic;

namespace RubricaTelefonicaAziendale.Entities;

public partial class Groups
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<People> Person { get; set; } = new List<People>();
}
