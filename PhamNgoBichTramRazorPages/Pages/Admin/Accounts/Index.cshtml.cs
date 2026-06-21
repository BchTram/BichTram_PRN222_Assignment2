using FUNewsManagement.Data.Models;
using FUNewsManagement.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PhamNgoBichTramRazorPages.Pages.Admin.Accounts;

public class IndexModel : PageModel
{
    private readonly IAccountRepository _accounts;

    public IndexModel(IAccountRepository accounts)
    {
        _accounts = accounts;
    }

    public List<SystemAccount> Accounts { get; private set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? Keyword { get; set; }

    [TempData]
    public string? Toast { get; set; }

    [BindProperty]
    public SystemAccount Account { get; set; } = new();

    [BindProperty]
    public string Mode { get; set; } = "Create";

    public async Task OnGetAsync()
    {
        Accounts = await _accounts.SearchAsync(Keyword);
    }

    public async Task<IActionResult> OnGetAccountAsync(short id)
    {
        var a = await _accounts.GetByIdAsync(id);
        return new JsonResult(a);
    }

    public async Task<IActionResult> OnPostSaveAsync()
    {
        if (string.IsNullOrWhiteSpace(Account.AccountEmail) || string.IsNullOrWhiteSpace(Account.AccountPassword))
        {
            Toast = "Email and password are required.";
            return RedirectToPage(new { keyword = Keyword });
        }

        if (Mode == "Create")
        {
            if (Account.AccountID <= 0)
            {
                Toast = "Account ID must be provided.";
                return RedirectToPage(new { keyword = Keyword });
            }

            await _accounts.CreateAsync(Account);
            Toast = "Account created.";
        }
        else
        {
            await _accounts.UpdateAsync(Account);
            Toast = "Account updated.";
        }

        return RedirectToPage(new { keyword = Keyword });
    }

    public async Task<IActionResult> OnPostDeleteAsync(short id)
    {
        await _accounts.DeleteAsync(id);
        Toast = "Account deleted.";
        return RedirectToPage(new { keyword = Keyword });
    }
}

