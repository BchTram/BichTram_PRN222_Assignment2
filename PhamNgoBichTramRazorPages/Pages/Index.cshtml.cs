using Microsoft.AspNetCore.Mvc.RazorPages;
using FUNewsManagement.Repository.Interfaces;
using FUNewsManagement.Data.Models;

namespace PhamNgoBichTramRazorPages.Pages;

public class IndexModel : PageModel
{
    private readonly INewsRepository _newsRepo;

    public IndexModel(INewsRepository newsRepo)
    {
        _newsRepo = newsRepo;
    }

    public List<NewsArticle> News { get; private set; } = new();

    public string? Keyword { get; set; }

    public async Task OnGetAsync(string? keyword)
    {
        Keyword = keyword;
        News = await _newsRepo.SearchAsync(keyword, onlyActive: true);
    }
}

