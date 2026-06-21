using FUNewsManagement.Data.Models;
using FUNewsManagement.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PhamNgoBichTramRazorPages.Pages.Admin.Report;

public class IndexModel : PageModel
{
    private readonly INewsRepository _news;

    public IndexModel(INewsRepository news)
    {
        _news = news;
    }

    [BindProperty(SupportsGet = true)]
    public DateTime? StartDate { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime? EndDate { get; set; }

    public List<NewsArticle>? Items { get; private set; }

    public async Task OnGetAsync()
    {
        if (StartDate is null || EndDate is null) return;

        var start = StartDate.Value.Date;
        var end = EndDate.Value.Date.AddDays(1).AddTicks(-1);
        Items = await _news.ReportByCreatedDateAsync(start, end);
    }
}

