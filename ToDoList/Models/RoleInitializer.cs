using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System;

namespace ToDoList.Models
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }

            string adminEmail = "admin@email.com";
            string adminPassword = "1qaz!QAZ";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                IdentityUser admin = new IdentityUser { Email = adminEmail, UserName = "admin@gmail.com", EmailConfirmed = true };
                IdentityResult result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }

            string userEmail = "user@email.com";
            string userPassword = "1qaz!QAZ";
            if (await userManager.FindByEmailAsync(userEmail) == null)
            {
                IdentityUser user = new IdentityUser { Email = userEmail, UserName = "user@gmail.com", EmailConfirmed = true };
                IdentityResult result = await userManager.CreateAsync(user, userPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "user");
                }
            }

        }
    }
}
