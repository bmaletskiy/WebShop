using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace WebShopDomain.model
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
    }
}
