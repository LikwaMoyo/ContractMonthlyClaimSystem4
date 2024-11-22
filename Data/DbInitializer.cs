// Data/DbInitializer.cs

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ContractMonthlyClaimSystem4.Data
{
    public static class DbInitializer
    {
        // Change return type from void to Task
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            try
            {
                // Get the role manager and user manager services
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

                // Define roles
                string[] roles = { "Lecturer", "Coordinator", "Manager" };

                // Create roles if they do not exist
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                // Create a test user
                var testUser = new IdentityUser
                {
                    UserName = "coordinator@example.com",
                    Email = "coordinator@example.com",
                    EmailConfirmed = true
                };

                // Check if the user already exists
                if (userManager.Users.All(u => u.UserName != testUser.UserName))
                {
                    var result = await userManager.CreateAsync(testUser, "Password123!");

                    if (result.Succeeded)
                    {
                        // Assign the Coordinator role to the user
                        await userManager.AddToRoleAsync(testUser, "Coordinator");
                    }
                    else
                    {
                        // Log or handle the errors
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"Error creating user: {error.Description}");
                        }
                    }
                }

                // Add a test lecturer user
                var lecturerUser = new IdentityUser
                {
                    UserName = "lecturer@example.com",
                    Email = "lecturer@example.com",
                    EmailConfirmed = true
                };

                // Check if the lecturer user already exists
                if (userManager.Users.All(u => u.UserName != lecturerUser.UserName))
                {
                    var result = await userManager.CreateAsync(lecturerUser, "Password123!");

                    if (result.Succeeded)
                    {
                        // Assign the Lecturer role to the user
                        await userManager.AddToRoleAsync(lecturerUser, "Lecturer");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework)
                Console.WriteLine($"An error occurred during database initialization: {ex.Message}");
                throw; // Rethrow to let the calling method handle it
            }
        }
    }
}
