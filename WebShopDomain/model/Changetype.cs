using System;
using System.Collections.Generic;

namespace WebShopDomain.Model;

public partial class Changetype : Entity
{

    public string Name { get; set; } = null!;

    public virtual ICollection<Orderhistory> Orderhistories { get; set; } = new List<Orderhistory>();
}
