using FUNewsManagement.Data.Models;
using FUNewsManagement.Repository.Interfaces;
using FUNewsManagement.Repository.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PhamNgoBichTramRazorPages.Security;

namespace PhamNgoBichTramRazorPages.Pages.Staff.Profile;
public class IndexModel : PageModel
{
    private readonly IAccountRepository _accounts;

    public IndexModel(IAccountRepository accounts)
    {
        _accounts = accounts;
    }

    [BindProperty]
    public SystemAccount? Account { get; set; }

    [TempData]
    public string? Toast { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var session = HttpContext.Session.GetJson<UserSession>(SessionKeys.UserSession);
        if (session?.Role != UserRole.Staff || session.AccountId is null) return RedirectToPage("/AccessDenied");

        Account = await _accounts.GetByIdAsync(session.AccountId.Value);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var session = HttpContext.Session.GetJson<UserSession>(SessionKeys.UserSession);
        if (session?.Role != UserRole.Staff || session.AccountId is null) return RedirectToPage("/AccessDenied");

        if (Account is null)
        {
            Toast = "Invalid data.";
            return RedirectToPage();
        }

        Account.AccountID = session.AccountId.Value;
        await _accounts.UpdateAsync(Account);
        Toast = "Profile updated.";
        return RedirectToPage();
    }
}


