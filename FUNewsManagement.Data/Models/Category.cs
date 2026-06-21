using System.ComponentModel.DataAnnotations;

namespace FUNewsManagement.Data.Models;

public sealed class Category
{
    [Key]
    public short CategoryID { get; set; }

    [Required]
    [StringLength(100)]
    public string CategoryName { get; set; } = string.Empty;

    [Required]
    [StringLength(250)]
    public string CategoryDesciption { get; set; } = string.Empty;

    public short? ParentCategoryID { get; set; }

    public bool? IsActive { get; set; }

    public Category? ParentCategory { get; set; }

    public ICollection<Category> Children { get; set; } = new List<Category>();

    public ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
}
