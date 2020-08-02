namespace Infrastructure.Identity.Seeder
{
    using Bogus;
    using Domain.Enums;
    using Infrastructure.Identity.Entities;
    using Microsoft.AspNetCore.Identity;
    using System.Linq;
    using System.Threading.Tasks;

    public static class BasicUserSeeder
    {
        public static async Task SeedDemoBasicUser(UserManager<ApplicationUser> userManager)
        {
            var firstName = new Faker().Name.FirstName();
            var lastName = new Faker().Name.LastName();

            var defaultUser = new ApplicationUser
            {
                UserName = $"{firstName.Substring(0, 1)}{lastName}",
                Email = "basicuser@gmail.com",
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
                    await userManager.CreateAsync(defaultUser, "this is a long password 2");
                    await userManager.AddToRoleAsync(defaultUser, Role.Basic.ToString());
                }

            }
        }
    }
}
