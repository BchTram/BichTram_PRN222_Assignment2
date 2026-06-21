using System.ComponentModel.DataAnnotations;

namespace FUNewsManagement.Data.Models;

public sealed class Tag
{
    [Key]
    public int TagID { get; set; }

    [StringLength(50)]
    public string? TagName { get; set; }

    [StringLength(400)]
    public string? Note { get; set; }

    public ICollection<NewsTag> NewsTags { get; set; } = new List<NewsTag>();
}
