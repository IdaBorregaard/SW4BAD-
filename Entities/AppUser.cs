using Microsoft.AspNetCore.Identity;

namespace assignment3.Entities;

public class AppUser : IdentityUser
{
    public int StaffId { get; set; }
}