using CoreApp.API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreApp.API.Infrastructure.Data.Configurations;

public class BookmarkConfiguration : IEntityTypeConfiguration<Bookmark>
{
  public void Configure(EntityTypeBuilder<Bookmark> builder)
  {
    // Table Name
    builder.ToTable("Bookmarks");

    // Primary Key
    builder.HasKey(b => b.Id);

    // Properties
    builder.Property(b => b.Title)
        .IsRequired()
        .HasMaxLength(255);

    builder.Property(b => b.Url)
        .IsRequired()
        .HasMaxLength(2048);

    builder.Property(b => b.AddDate);

    builder.Property(b => b.Icon)
        .HasMaxLength(2048);

    // Relationships
    builder.HasOne(b => b.BookmarkFolder)
        .WithMany(f => f.Bookmarks)
        .HasForeignKey(b => b.BookmarkFolderId)
        .OnDelete(DeleteBehavior.Cascade);
  }
}
