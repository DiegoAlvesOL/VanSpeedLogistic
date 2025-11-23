using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using VanSpeedLogistics.Models;

namespace VanSpeedLogistics.Utilities;

public static class DbInitializer
{
    /// <summary>
    /// Método criado para garantir que os papéis existam 
    /// </summary>
    /// <param name="roleManager"></param>
    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("Manager"))
        {
            await roleManager.CreateAsync(new IdentityRole("Manager"));
        }

        if (!await roleManager.RoleExistsAsync("Operator"))
        {
            await roleManager.CreateAsync(new IdentityRole("Operator"));
        }
    }


    /// <summary>
    /// Método para criar o usuário do Peter que recebe o papel Manager
    /// </summary>
    /// <param name="userManager"></param>
    public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
    {
        if (await userManager.FindByEmailAsync("peter@vanspeed.com") == null)
        {
            var peterUser = new ApplicationUser()
            {
                UserName = "peter.vanspeed",
                Email = "peter@vanspeed.com",
                FullName = "Peter Parker",
                EmailConfirmed = true,
            };
            var result = await userManager.CreateAsync(peterUser, "VanSpeed@2025");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(peterUser, "Manager");
            }
        }
    }
}