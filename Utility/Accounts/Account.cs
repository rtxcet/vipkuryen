using Microsoft.AspNetCore.Identity;
using vipkuryen.Models;

namespace vipkuryen.Utility.Accounts
{
    public static class Account
    {
        public static async Task SeedDefaultUser(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminRole = "Admin";
            string adminEmail = "admin@example.com";
            string adminPassword = "admin123";

            // Admin rolü yoksa ekle
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            // Eğer sistemde hiç kullanıcı yoksa admin oluştur
            if (!userManager.Users.Any())
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "Admin",
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }
            }
        }
    }
}