using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ContractMonthlyClaimSystem4.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            try
            {
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

                // Define roles
                string[] roles = { "Lecturer", "Coordinator", "Manager", "HR" };

                // Create roles if they do not exist
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                // Create a test HR user
                var hrUserEmail = "hr@example.com";
                var hrUser = new IdentityUser
                {
                    UserName = hrUserEmail,
                    Email = hrUserEmail,
                    EmailConfirmed = true
                };

                if (userManager.Users.All(u => u.UserName != hrUserEmail))
                {
                    var result = await userManager.CreateAsync(hrUser, "Password123!");

                    if (result.Succeeded)
                    {
                        // Assign the HR role to the user
                        await userManager.AddToRoleAsync(hrUser, "HR");
                    }
                    else
                    {
                        // Log or handle the errors
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"Error creating HR user: {error.Description}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during database initialization: {ex.Message}");
                throw;
            }
        }
    }
}
