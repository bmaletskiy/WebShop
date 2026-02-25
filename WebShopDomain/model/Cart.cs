using System;
using System.Collections.Generic;

namespace WebShopDomain.Model;

public partial class Cart : Entity
{

    public int? Customerid { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual ICollection<Cartitem> Cartitems { get; set; } = new List<Cartitem>();

    public virtual Customer? Customer { get; set; }
}
