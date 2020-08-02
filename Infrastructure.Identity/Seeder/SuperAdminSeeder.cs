namespace Infrastructure.Identity.Seeder
{
    using AutoBogus;
    using Bogus;
    using Domain.Entities;
    using Domain.Enums;
    using Infrastructure.Identity.Entities;
    using Microsoft.AspNetCore.Identity;
    using System.Linq;
    using System.Threading.Tasks;

    public static class SuperAdminSeeder
    {
        public static async Task SeedDemoSuperAdminsAsync(UserManager<ApplicationUser> userManager)
        {
            var firstName = new Faker().Name.FirstName();
            var lastName = new Faker().Name.LastName();

            var defaultUser = new ApplicationUser
            {
                UserName = $"{firstName.Substring(0, 1)}{lastName}",
                Email = "superadmin@gmail.com",
                FirstName = firstName,
                LastName = lastName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    var result = await userManager.CreateAsync(defaultUser, "this is a long password 2");
                    await userManager.AddToRoleAsync(defaultUser, Role.Basic.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Role.Moderator.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Role.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Role.SuperAdmin.ToString());
                }
            }
        }
    }
}
