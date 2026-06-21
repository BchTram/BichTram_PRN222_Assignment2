using FUNewsManagement.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagement.Data;

public sealed class FUNewsManagementDbContext : DbContext
{
    public FUNewsManagementDbContext(DbContextOptions<FUNewsManagementDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<NewsArticle> NewsArticles => Set<NewsArticle>();
    public DbSet<NewsTag> NewsTags => Set<NewsTag>();
    public DbSet<SystemAccount> SystemAccounts => Set<SystemAccount>();
    public DbSet<Tag> Tags => Set<Tag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(e =>
        {
            e.ToTable("Category");
            e.Property(x => x.CategoryID).HasColumnName("CategoryID");
            e.Property(x => x.CategoryName).HasColumnName("CategoryName");
            e.Property(x => x.CategoryDesciption).HasColumnName("CategoryDesciption");
            e.Property(x => x.ParentCategoryID).HasColumnName("ParentCategoryID");
            e.Property(x => x.IsActive).HasColumnName("IsActive");

            e.HasOne(x => x.ParentCategory)
                .WithMany(x => x!.Children)
                .HasForeignKey(x => x.ParentCategoryID)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<SystemAccount>(e =>
        {
            e.ToTable("SystemAccount");
            e.Property(x => x.AccountID).HasColumnName("AccountID").ValueGeneratedNever();
            e.Property(x => x.AccountName).HasColumnName("AccountName");
            e.Property(x => x.AccountEmail).HasColumnName("AccountEmail");
            e.Property(x => x.AccountRole).HasColumnName("AccountRole");
            e.Property(x => x.AccountPassword).HasColumnName("AccountPassword");
        });

        modelBuilder.Entity<Tag>(e =>
        {
            e.ToTable("Tag");
            e.Property(x => x.TagID).HasColumnName("TagID").ValueGeneratedNever();
            e.Property(x => x.TagName).HasColumnName("TagName");
            e.Property(x => x.Note).HasColumnName("Note");
        });

        modelBuilder.Entity<NewsArticle>(e =>
        {
            e.ToTable("NewsArticle");
            e.Property(x => x.NewsArticleID).HasColumnName("NewsArticleID");
            e.Property(x => x.NewsTitle).HasColumnName("NewsTitle");
            e.Property(x => x.Headline).HasColumnName("Headline");
            e.Property(x => x.CreatedDate).HasColumnName("CreatedDate");
            e.Property(x => x.NewsContent).HasColumnName("NewsContent");
            e.Property(x => x.NewsSource).HasColumnName("NewsSource");
            e.Property(x => x.CategoryID).HasColumnName("CategoryID");
            e.Property(x => x.NewsStatus).HasColumnName("NewsStatus");
            e.Property(x => x.CreatedByID).HasColumnName("CreatedByID");
            e.Property(x => x.UpdatedByID).HasColumnName("UpdatedByID");
            e.Property(x => x.ModifiedDate).HasColumnName("ModifiedDate");

            e.HasOne(x => x.Category)
                .WithMany(x => x.NewsArticles)
                .HasForeignKey(x => x.CategoryID)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.CreatedBy)
                .WithMany(x => x.CreatedNewsArticles)
                .HasForeignKey(x => x.CreatedByID)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<NewsTag>(e =>
        {
            e.ToTable("NewsTag");
            e.HasKey(x => new { x.NewsArticleID, x.TagID });
            e.Property(x => x.NewsArticleID).HasColumnName("NewsArticleID");
            e.Property(x => x.TagID).HasColumnName("TagID");

            e.HasOne(x => x.NewsArticle)
                .WithMany(x => x.NewsTags)
                .HasForeignKey(x => x.NewsArticleID)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Tag)
                .WithMany(x => x.NewsTags)
                .HasForeignKey(x => x.TagID)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

