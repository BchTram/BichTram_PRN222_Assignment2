using System.ComponentModel.DataAnnotations;

namespace FUNewsManagement.Data.Models;

public sealed class SystemAccount
{
    [Key]
    public short AccountID { get; set; }

    [StringLength(100)]
    public string? AccountName { get; set; }

    [StringLength(70)]
    public string? AccountEmail { get; set; }

    public int? AccountRole { get; set; } // Staff = 1, Lecturer = 2

    [StringLength(70)]
    public string? AccountPassword { get; set; }

    public ICollection<NewsArticle> CreatedNewsArticles { get; set; } = new List<NewsArticle>();
}
