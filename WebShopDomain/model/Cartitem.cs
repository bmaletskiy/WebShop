using System;
using System.Collections.Generic;

namespace WebShopDomain.Model;

public partial class Cartitem : Entity
{

    public int? Cartid { get; set; }

    public int? Productid { get; set; }

    public int? Quantity { get; set; }

    public virtual Cart? Cart { get; set; }

    public virtual Product? Product { get; set; }
}
