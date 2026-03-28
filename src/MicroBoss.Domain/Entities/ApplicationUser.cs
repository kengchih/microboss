using Microsoft.AspNetCore.Identity;

namespace MicroBoss.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public bool IsEnabled { get; set; } = true;
    public DateTime? LastLoginTime { get; set; }
}
