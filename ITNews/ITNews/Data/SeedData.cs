using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITNews.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ITNews.Data
{
    public class SeedData
    {
        public static async Task InitializeAsync(
            IServiceProvider services)
        {
            var configuration = services.GetRequiredService<IConfiguration>();
            var roleManager = services
                .GetRequiredService<RoleManager<IdentityRole>>();
            await CreateRolesAsync(roleManager, configuration);

            var userManager = services
                .GetRequiredService<UserManager<User>>();
            await CreateAdminAsync(userManager, configuration);

        }

        private static async Task CreateRolesAsync(
            RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            var role = configuration["AdminAccount:role"];
            var alreadyExists = await roleManager
                .RoleExistsAsync(role);

            if (alreadyExists) return;

            await roleManager.CreateAsync(
                new IdentityRole(role));
        }

        private static async Task CreateAdminAsync(
            UserManager<User> userManager, IConfiguration configuration)
        {
            var nameAdmin = configuration["AdminAccount:username"];
            var passwordAdmin = configuration["AdminAccount:password"];
            var emailAdmin = configuration["AdminAccount:email"];
            var roleAdmin = configuration["AdminAccount:role"];

            var admin = await userManager.Users
                .Where(x => x.UserName == nameAdmin)
                .SingleOrDefaultAsync();

            if (admin != null) return;

            admin = new User
            {
                UserName = nameAdmin,
                Email = emailAdmin
            };
            await userManager.CreateAsync(
                admin, passwordAdmin);
            await userManager.AddToRoleAsync(
                admin, roleAdmin);
        }
    }
}
