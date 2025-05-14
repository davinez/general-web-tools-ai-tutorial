using CoreApp.API.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CoreApp.API.Infrastructure.Data.Configurations;

public class BookmarkFolderConfiguration : IEntityTypeConfiguration<BookmarkFolder>
{
  public void Configure(EntityTypeBuilder<BookmarkFolder> builder)
  {
    // Table Name
    builder.ToTable("BookmarkFolders");

    // Primary Key
    builder.HasKey(f => f.Id);

    // Properties
    builder.Property(f => f.Title)
        .IsRequired()
        .HasMaxLength(255);

    builder.Property(f => f.AddDate)
        .HasColumnType("datetime");

    builder.Property(f => f.LastModified)
        .HasColumnType("datetime");

    // Relationships
    builder.HasMany(f => f.SubFolders)
        .WithOne(f => f.ParentFolder)
        .HasForeignKey(f => f.ParentFolderId)
        .OnDelete(DeleteBehavior.Restrict);

    builder.HasMany(f => f.Bookmarks)
        .WithOne(b => b.BookmarkFolder)
        .HasForeignKey(b => b.BookmarkFolderId)
        .OnDelete(DeleteBehavior.Cascade);
  }
}
