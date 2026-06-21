using System.ComponentModel.DataAnnotations;

namespace FUNewsManagement.Data.Models;

public sealed class NewsArticle
{
    [Key]
    [StringLength(20)]
    public string NewsArticleID { get; set; } = string.Empty;

    [StringLength(400)]
    public string? NewsTitle { get; set; }

    [Required]
    [StringLength(150)]
    public string Headline { get; set; } = string.Empty;

    public DateTime? CreatedDate { get; set; }

    [StringLength(4000)]
    public string? NewsContent { get; set; }

    [StringLength(400)]
    public string? NewsSource { get; set; }

    public short? CategoryID { get; set; }

    public bool? NewsStatus { get; set; } // active(1)/inactive(0)

    public short? CreatedByID { get; set; }

    public short? UpdatedByID { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public Category? Category { get; set; }

    public SystemAccount? CreatedBy { get; set; }

    public ICollection<NewsTag> NewsTags { get; set; } = new List<NewsTag>();
}
