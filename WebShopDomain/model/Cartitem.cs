using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebShopDomain.Model;

public partial class Cartitem : Entity
{

    public int? Cartid { get; set; }
    public int? Productid { get; set; }
    [Display(Name = "Кількість")]
    public int? Quantity { get; set; }
    public virtual Cart? Cart { get; set; }
    [Display(Name ="Товар")]
    public virtual Product? Product { get; set; }
}
