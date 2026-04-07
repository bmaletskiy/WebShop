using Microsoft.AspNetCore.Identity;
using WebShop.Models;

namespace WebShopInfrastructure
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@gmail.com";
            string password = "Admin_1234";

            if (await roleManager.FindByNameAsync("admin") == null)
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole("admin"));
                Console.WriteLine($"Роль admin створена: {roleResult.Succeeded}");
            }

            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }

            if (await roleManager.FindByNameAsync("manager") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("manager"));
            }

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                Console.WriteLine("Створюємо адміна...");
                User admin = new User { Email = adminEmail, UserName = adminEmail };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                Console.WriteLine($"Адмін створений: {result.Succeeded}");

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Помилка: {error.Description}");
                    }
                }
                else
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                    Console.WriteLine("Роль присвоєна адміну");
                }
            }
            else
            {
                Console.WriteLine("Адмін вже існує");
            }
        }
    }
}