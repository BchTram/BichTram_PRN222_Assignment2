using FUNewsManagement.Data.Models;
using FUNewsManagement.Repository.Interfaces;
using FUNewsManagement.Repository.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using PhamNgoBichTramRazorPages.Hubs;
using PhamNgoBichTramRazorPages.Security;

namespace PhamNgoBichTramRazorPages.Pages.Staff.News;

public class IndexModel : PageModel
{
    private readonly INewsRepository _news;
    private readonly ICategoryRepository _categories;
    private readonly ITagRepository _tags;
    private readonly IHubContext<NewsHub> _hub;

    public IndexModel(INewsRepository news, ICategoryRepository categories, ITagRepository tags, IHubContext<NewsHub> hub)
    {
        _news = news;
        _categories = categories;
        _tags = tags;
        _hub = hub;
    }

    public List<NewsArticle> News { get; private set; } = new();
    public List<Category> AllCategories { get; private set; } = new();
    public List<Tag> AllTags { get; private set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? Keyword { get; set; }

    [TempData]
    public string? Toast { get; set; }

    [BindProperty]
    public NewsArticle Article { get; set; } = new();

    [BindProperty]
    public List<int> TagIds { get; set; } = new();

    [BindProperty]
    public string Mode { get; set; } = "Create";

    public async Task OnGetAsync()
    {
        AllCategories = await _categories.SearchAsync(null);
        AllTags = await _tags.GetAllAsync();
        News = await _news.SearchAsync(Keyword, onlyActive: null);
    }

    public async Task<IActionResult> OnGetNewsAsync(string id)
    {
        var n = await _news.GetByIdAsync(id);
        if (n is null) return new JsonResult(new { });
        return new JsonResult(new
        {
            n.NewsArticleID,
            n.NewsTitle,
            n.Headline,
            n.NewsContent,
            n.NewsSource,
            n.CategoryID,
            n.NewsStatus,
            TagIds = n.NewsTags.Select(t => t.TagID).ToList()
        });
    }

    public async Task<IActionResult> OnPostSaveAsync()
    {
        var session = HttpContext.Session.GetJson<UserSession>(SessionKeys.UserSession);
        if (session?.Role != UserRole.Staff || session.AccountId is null)
        {
            return RedirectToPage("/AccessDenied");
        }

        if (string.IsNullOrWhiteSpace(Article.NewsArticleID) || string.IsNullOrWhiteSpace(Article.Headline))
        {
            Toast = "News ID and headline are required.";
            return RedirectToPage(new { keyword = Keyword });
        }

        var now = DateTime.Now;
        if (Mode == "Create")
        {
            Article.CreatedByID = session.AccountId.Value;
            Article.UpdatedByID = session.AccountId.Value;
            Article.CreatedDate = now;
            Article.ModifiedDate = now;
        }
        else
        {
            var existing = await _news.GetByIdAsync(Article.NewsArticleID);
            if (existing is not null)
            {
                Article.CreatedByID = existing.CreatedByID;
                Article.CreatedDate = existing.CreatedDate;
            }

            Article.UpdatedByID = session.AccountId.Value;
            Article.ModifiedDate = now;
        }

        await _news.CreateOrUpdateAsync(Article, TagIds);
        await _hub.Clients.All.SendAsync("NewsChanged");
        Toast = Mode == "Create" ? "News created." : "News updated.";
        return RedirectToPage(new { keyword = Keyword });
    }

    public async Task<IActionResult> OnPostDeleteAsync(string id)
    {
        await _news.DeleteAsync(id);
        await _hub.Clients.All.SendAsync("NewsChanged");
        Toast = "News deleted.";
        return RedirectToPage(new { keyword = Keyword });
    }
}


