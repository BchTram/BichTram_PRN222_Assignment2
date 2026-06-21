using System.ComponentModel.DataAnnotations;
using FUNewsManagement.Repository.Interfaces;
using FUNewsManagement.Repository.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PhamNgoBichTramRazorPages.Security;

namespace PhamNgoBichTramRazorPages.Pages;

public class LoginModel : PageModel
{
    private readonly IConfiguration _config;
    private readonly IAccountRepository _accounts;

    public LoginModel(IConfiguration config, IAccountRepository accounts)
    {
        _config = config;
        _accounts = accounts;
    }

    [BindProperty]
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    [Required]
    public string Password { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }

    public IActionResult OnGet()
    {
        // Already logged in => go to appropriate home
        var current = HttpContext.Session.GetJson<UserSession>(SessionKeys.UserSession);
        if (current is not null)
        {
            return RedirectToRoleHome(current.Role);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var admin = _config.GetSection(AdminAccountOptions.SectionName).Get<AdminAccountOptions>() ?? new AdminAccountOptions();
        if (string.Equals(Email, admin.Email, StringComparison.OrdinalIgnoreCase) && Password == admin.Password)
        {
            HttpContext.Session.SetJson(SessionKeys.UserSession, new UserSession
            {
                AccountId = null,
                Email = admin.Email,
                DisplayName = admin.DisplayName,
                Role = UserRole.Admin
            });

            return RedirectToRoleHome(UserRole.Admin);
        }

        var account = await _accounts.LoginStaffOrLecturerAsync(Email, Password);
        if (account is null || account.AccountRole is null)
        {
            ErrorMessage = "Invalid email or password.";
            return Page();
        }

        var role = account.AccountRole == 1 ? UserRole.Staff : UserRole.Lecturer;
        HttpContext.Session.SetJson(SessionKeys.UserSession, new UserSession
        {
            AccountId = account.AccountID,
            Email = account.AccountEmail ?? Email,
            DisplayName = account.AccountName ?? account.AccountEmail ?? Email,
            Role = role
        });

        return RedirectToRoleHome(role);
    }

    private IActionResult RedirectToRoleHome(UserRole role) =>
        role switch
        {
            UserRole.Admin => RedirectToPage("/Admin/Accounts/Index"),
            UserRole.Staff => RedirectToPage("/Staff/News/Index"),
            _ => RedirectToPage("/Index")
        };
}


