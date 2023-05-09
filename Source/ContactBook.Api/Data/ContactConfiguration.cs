using ContactBook.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactBook.Api.Data;

public sealed class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .HasMaxLength(75);

        // RFC 5321 specification
        builder.Property(x => x.Email)
            .HasMaxLength(254);
        
        // E.164 format
        builder.Property(x => x.Mobile)
            .HasMaxLength(15);
        
        builder
            .HasIndex(c => new { c.Name, c.Email, c.Mobile })
            .IsUnique();
    }
}