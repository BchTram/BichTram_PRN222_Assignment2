using FUNewsManagement.Data.Models;
using FUNewsManagement.Repository.Interfaces;
using FUNewsManagement.Repository.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PhamNgoBichTramRazorPages.Security;

namespace PhamNgoBichTramRazorPages.Pages.Staff.History;

public class IndexModel : PageModel
{
    private readonly INewsRepository _news;

    public IndexModel(INewsRepository news)
    {
        _news = news;
    }

    public List<NewsArticle> News { get; private set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var session = HttpContext.Session.GetJson<UserSession>(SessionKeys.UserSession);
        if (session?.Role != UserRole.Staff || session.AccountId is null) return RedirectToPage("/AccessDenied");

        News = await _news.GetByCreatorAsync(session.AccountId.Value);
        return Page();
    }
}
