namespace MicroBoss.Application.DTOs;

public class AccountDto
{
    public string Id { get; set; } = string.Empty;
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public bool IsEnabled { get; set; }
    public DateTime? LastLoginTime { get; set; }
    public string? RoleName { get; set; }
}

public class CreateAccountDto
{
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string Password { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public bool IsEnabled { get; set; } = true;
}

public class UpdateAccountDto
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? RoleName { get; set; }
    public bool? IsEnabled { get; set; }
}

public class RoleDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
