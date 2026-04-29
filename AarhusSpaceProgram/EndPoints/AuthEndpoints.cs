using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using assignment3.Entities;

namespace assignment3.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/api/auth/login", async (LoginRequest request, UserManager<AppUser> userManager, IConfiguration config) =>
        {
            // 1. Find the user by username
            var user = await userManager.FindByNameAsync(request.Username);
            if (user == null) return Results.Unauthorized();

            // 2. Check the password
            var isValidPassword = await userManager.CheckPasswordAsync(user, request.Password);
            if (!isValidPassword) return Results.Unauthorized();

            // 3. Get the user's roles
            var roles = await userManager.GetRolesAsync(user);

            // 4. Build the JWT token
            var jwtSettings = config.GetSection("JwtSettings");
            var secret = jwtSettings["Secret"]!;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            // Add each role as a claim
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Results.Ok(new { token = tokenString });
        });
    }
}

public record LoginRequest(string Username, string Password);