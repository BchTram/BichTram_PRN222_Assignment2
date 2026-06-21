using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PhamNgoBichTramRazorPages.Security;

namespace PhamNgoBichTramRazorPages.Pages;

public class LogoutModel : PageModel
{
    public IActionResult OnGet()
    {
        HttpContext.Session.Remove(SessionKeys.UserSession);
        return RedirectToPage("/Login");
    }
}


