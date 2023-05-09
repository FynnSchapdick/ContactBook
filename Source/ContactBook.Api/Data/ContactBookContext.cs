using ContactBook.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace ContactBook.Api.Data;

public sealed class ContactBookContext : DbContext
{
    public DbSet<Contact> Contacts { get; set; }

    public ContactBookContext(DbContextOptions<ContactBookContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ContactConfiguration());
    }
}