namespace FUNewsManagement.Repository.Models;

public enum UserRole
{
    Admin = 0,
    Staff = 1,
    Lecturer = 2
}

public sealed class UserSession
{
    public short? AccountId { get; init; }
    public string Email { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public UserRole Role { get; init; }
}

