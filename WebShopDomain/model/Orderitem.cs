using System;
using System.Collections.Generic;

namespace WebShopDomain.Model;

public partial class Orderitem : Entity
{

    public int? Orderid { get; set; }

    public int? Productid { get; set; }

    public int Quantity { get; set; }

    public decimal Unitprice { get; set; }

    public decimal Totalprice { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}
