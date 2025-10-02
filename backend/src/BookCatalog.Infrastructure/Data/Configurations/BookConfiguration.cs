using BookCatalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookCatalog.Infrastructure.Data.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.Subtitle)
            .HasMaxLength(300);

        builder.Property(b => b.Description)
            .HasMaxLength(5000);

        builder.Property(b => b.ISBN)
            .HasMaxLength(17); 

        builder.Property(b => b.PageCount)
            .HasColumnType("int");

        builder.Property(b => b.Publisher)
            .HasMaxLength(200);


        builder.HasOne(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

       
        builder.HasOne(b => b.Genre)
            .WithMany(g => g.Books)
            .HasForeignKey(b => b.GenreId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.HasIndex(b => b.Title);
        //builder.HasIndex(b => b.ISBN).IsUnique();
        builder.HasIndex(b => b.AuthorId);
        builder.HasIndex(b => b.GenreId);
    }
}
