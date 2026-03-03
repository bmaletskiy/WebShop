using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebShopDomain.Model;

public partial class Category : Entity
{
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Категорія")]
    public string Categoryname { get; set; } = null!;

    [Display(Name = "Інформація про категорію")]
    public string? Categoryinfo { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
