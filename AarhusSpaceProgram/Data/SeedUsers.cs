using Microsoft.AspNetCore.Identity;
using assignment3.Entities;

namespace assignment3.Data;

public static class SeedUsers
{
    public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        Console.WriteLine("Starting user seeding...");

        // 1. Create roles if they don't exist
        string[] roles = { "Astronaut", "Scientist", "Manager" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        Console.WriteLine("Roles created. Creating users...");

        // 2. Seed users
        await CreateUser(userManager, "manager1", "Manager1Pass!", "Manager", 1);
        await CreateUser(userManager, "scientist1", "Scientist1Pass!", "Scientist", 101);
        await CreateUser(userManager, "astronaut1", "Astronaut1Pass!", "Astronaut", 201);
    }

    private static async Task CreateUser(UserManager<AppUser> userManager, string username, string password, string role, int staffId)
    {
        Console.WriteLine($"Creating user: {username}");

        if (await userManager.FindByNameAsync(username) == null)
        {
            var user = new AppUser
            {
                UserName = username,
                StaffId = staffId
            };

            var result = await userManager.CreateAsync(user, password);
            Console.WriteLine($"Create result: {result.Succeeded}, Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}");

            await userManager.AddToRoleAsync(user, role);
        }
        else
        {
            Console.WriteLine($"User {username} already exists.");
        }
    }
}