using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITNews.Configurations;
using ITNews.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ITNews.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var configuration = services.GetRequiredService<IConfiguration>();

            var rolesSettings = configuration.GetSection("Roles").Get<RolesSettings>();
            var adminSettings = configuration.GetSection("AdminAccount").Get<AdminSettings>();

            var roleManager = services
                .GetRequiredService<RoleManager<IdentityRole>>();
            await CreateRolesAsync(roleManager, rolesSettings);

            var userManager = services
                .GetRequiredService<UserManager<ApplicationUser>>();
            await CreateAdminAsync(userManager, adminSettings);

        }

        private static async Task CreateRolesAsync(
            RoleManager<IdentityRole> roleManager, RolesSettings rolesSettings)
        {
            var roles = rolesSettings.ListOfRoles;

            foreach (var role in roles)
            {
                if (await roleManager.RoleExistsAsync(role))
                    continue;
                await roleManager.CreateAsync(
                    new IdentityRole(role));
            }
        }

        private static async Task CreateAdminAsync(
            UserManager<ApplicationUser> userManager, AdminSettings adminSettings)
        {
            //var nameAdmin = configuration["AdminAccount:username"];
            //var passwordAdmin = configuration["AdminAccount:password"];
            //var emailAdmin = configuration["AdminAccount:email"];
            //var roleAdmin = configuration["AdminAccount:role"];

            var nameAdmin = adminSettings.UserName;
            var passwordAdmin = adminSettings.Password;
            var emailAdmin = adminSettings.Email;
            var roleAdmin = adminSettings.Role;



            var admin = await userManager.FindByNameAsync(nameAdmin);

            if (admin != null) return;

            admin = new ApplicationUser
            {
                UserName = nameAdmin,
                Email = emailAdmin,
                LockoutEnabled = false,
                CreatedAt =  DateTime.Now,
                UserBlocked = false
            };

            var result = userManager.CreateAsync(admin, passwordAdmin).Result;
            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(
                admin, roleAdmin).Wait();

            }

        }
    }
}
