using Microsoft.AspNetCore.Identity;

namespace WebShop.Models
{
    public class User : IdentityUser
    {
        public DateOnly? BirthDate { get; set; }
    }
}