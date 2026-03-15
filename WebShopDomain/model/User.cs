using Microsoft.AspNetCore.Identity;

namespace WebShop.Models
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
    }
}