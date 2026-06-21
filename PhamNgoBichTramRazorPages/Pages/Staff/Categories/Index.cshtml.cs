using FUNewsManagement.Data.Models;
using FUNewsManagement.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PhamNgoBichTramRazorPages.Pages.Staff.Categories;

public class IndexModel : PageModel
{
    private readonly ICategoryRepository _categories;

    public IndexModel(ICategoryRepository categories)
    {
        _categories = categories;
    }

    public List<Category> Categories { get; private set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? Keyword { get; set; }

    [TempData]
    public string? Toast { get; set; }

    [BindProperty]
    public Category Category { get; set; } = new();

    [BindProperty]
    public string Mode { get; set; } = "Create";

    public async Task OnGetAsync()
    {
        Categories = await _categories.SearchAsync(Keyword);
    }

    public async Task<IActionResult> OnGetCategoryAsync(short id)
    {
        var c = await _categories.GetByIdAsync(id);
        return new JsonResult(c);
    }

    public async Task<IActionResult> OnPostSaveAsync()
    {
        if (string.IsNullOrWhiteSpace(Category.CategoryName) || string.IsNullOrWhiteSpace(Category.CategoryDesciption))
        {
            Toast = "Name and description are required.";
            return RedirectToPage(new { keyword = Keyword });
        }

        if (Mode == "Create")
        {
            Category.CategoryID = 0; // identity
            await _categories.CreateAsync(Category);
            Toast = "Category created.";
        }
        else
        {
            await _categories.UpdateAsync(Category);
            Toast = "Category updated.";
        }

        return RedirectToPage(new { keyword = Keyword });
    }

    public async Task<IActionResult> OnPostDeleteAsync(short id)
    {
        if (!await _categories.CanDeleteAsync(id))
        {
            Toast = "Cannot delete: this category is used by news articles.";
            return RedirectToPage(new { keyword = Keyword });
        }

        await _categories.DeleteAsync(id);
        Toast = "Category deleted.";
        return RedirectToPage(new { keyword = Keyword });
    }
}

