namespace Infrastructure.Identity.Seeder
{
    using Domain.Enums;
    using Microsoft.AspNetCore.Identity;
    using System.Threading.Tasks;

    public static class RoleSeeder
    {
        public static async Task SeedDemoRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Role.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Role.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Role.Moderator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Role.Basic.ToString()));
        }
    }
}
