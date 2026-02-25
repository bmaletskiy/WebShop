using System;
using System.Collections.Generic;

namespace WebShopDomain.Model;

public partial class Orderhistory : Entity
{

    public int? Orderid { get; set; }

    public DateTime? Changedat { get; set; }

    public string? Oldvalue { get; set; }

    public string? Newvalue { get; set; }

    public int? Changedtypeid { get; set; }

    public virtual Changetype? Changedtype { get; set; }

    public virtual Order? Order { get; set; }
}
