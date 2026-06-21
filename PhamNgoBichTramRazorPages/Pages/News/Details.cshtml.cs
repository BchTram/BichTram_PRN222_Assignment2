using FUNewsManagement.Data.Models;
using FUNewsManagement.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace PhamNgoBichTramRazorPages.Pages.News;

public class DetailsModel : PageModel
{
    private readonly INewsRepository _newsRepo;

    public DetailsModel(INewsRepository newsRepo)
    {
        _newsRepo = newsRepo;
    }

    public NewsArticle? Article { get; private set; }

    public async Task<IActionResult> OnGetAsync(string id)
    {
        Article = await _newsRepo.GetByIdAsync(id);
        if (Article is null) return Page();

        // Public view: only active news should be visible
        if (Article.NewsStatus != true)
        {
            Article = null;
        }

        return Page();
    }
}


