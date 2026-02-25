using System;
using System.Collections.Generic;

namespace WebShopDomain.Model;

public partial class Order : Entity
{

    public int? Customerid { get; set; }

    public int? Orderstatusid { get; set; }

    public DateTime? Orderdate { get; set; }

    public decimal? Totalamount { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<Orderhistory> Orderhistories { get; set; } = new List<Orderhistory>();

    public virtual ICollection<Orderitem> Orderitems { get; set; } = new List<Orderitem>();

    public virtual Orderstatus? Orderstatus { get; set; }
}
