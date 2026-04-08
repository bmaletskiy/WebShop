using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopDomain.model
{
    public class CategorySalesItem
    {
        public string CategoryName { get; set; }
        public int Count { get; set; }

        public CategorySalesItem(string categoryName, int count)
        {
            CategoryName = categoryName;
            Count = count;
        }
    }
}
