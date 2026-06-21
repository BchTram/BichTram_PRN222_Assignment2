namespace PhamNgoBichTramRazorPages.Security;

public sealed class AdminAccountOptions
{
    public const string SectionName = "AdminAccount";

    public string Email { get; set; } = "admin@FUNewsManagementSystem.org";
    public string Password { get; set; } = "@@abc123@@";
    public string DisplayName { get; set; } = "Admin";
}


