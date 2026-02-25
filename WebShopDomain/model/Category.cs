using System;
using System.Collections.Generic;

namespace WebShopDomain.Model;

public partial class Category : Entity
{

    public string Categoryname { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
