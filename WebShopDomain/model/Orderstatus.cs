using System;
using System.Collections.Generic;

namespace WebShopDomain.Model;

public partial class Orderstatus : Entity
{

    public string Statusname { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
