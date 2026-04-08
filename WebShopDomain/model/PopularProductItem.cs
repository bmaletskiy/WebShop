using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopDomain.model
{
    public class PopularProductItem
    {
        public string Name { get; set; }
        public int Count { get; set; }

        public PopularProductItem(string name, int count)
        {
            Name = name;
            Count = count;
        }
    }
}
