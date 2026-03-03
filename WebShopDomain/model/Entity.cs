using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WebShopDomain.Model
{
    public abstract class Entity
    {
        public int Id { get; set; }
    }
}
