using System.Data.Common;
using ContactBook.Api.Data;
using ContactBook.Api.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ContactBook.Api.IntegrationTests;

public sealed class ContactBookApiFactory : WebApplicationFactory<AssemblyMarker>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ContactBookContext>));
            services.RemoveAll(typeof(ContactBookContext));
            
            services.AddSingleton<DbConnection>(container =>
            {
                SqliteConnection connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();
                return connection;
            });

            services.AddDbContext<ContactBookContext>((provider, options) =>
            {
                var connection = provider.GetRequiredService<DbConnection>();
                options.UseSqlite(connection);
                options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
        });
    }

    public async Task ArrangeForEndpointTesting(params Contact[] contacts)
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        ContactBookContext context = scope.ServiceProvider.GetRequiredService<ContactBookContext>();
        context.Contacts.AddRange(contacts);
        await context.SaveChangesAsync();
    }
}