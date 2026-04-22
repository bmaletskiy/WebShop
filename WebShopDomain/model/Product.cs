using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;

namespace WebShopDomain.Model;

public partial class Product : Entity
{
    [Display(Name = "Категорія")]
    public int? Categoryid { get; set; }
    [Display(Name = "Назва")]
    public string Name { get; set; } = null!;
    [Display(Name = "Опис")]
    public string? Description { get; set; }
    [Display(Name = "Ціна")]
    public decimal? Price { get; set; }
    [Display(Name = "Кількість")]
    public int Availableqty { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }


    public string? ImageUrl { get; set; } // шлях до картинки

    [NotMapped]
    public IFormFile? ImageFile { get; set; } // для форми

    public virtual ICollection<Cartitem> Cartitems { get; set; } = new List<Cartitem>();
    [Display(Name="Категорія")]
    public virtual Category? Category { get; set; }

    public virtual ICollection<Orderitem> Orderitems { get; set; } = new List<Orderitem>();
}
