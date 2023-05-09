using ContactBook.Api.Data;
using ContactBook.Api.Endpoints.CreateContact;
using ContactBook.Api.Endpoints.DeleteContact;
using ContactBook.Api.Endpoints.GetContact;
using ContactBook.Api.Endpoints.GetContacts;
using ContactBook.Api.Endpoints.UpdateContact;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ContactBook.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseApi(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        app.MapCreateContactEndpoint();
        app.MapGetContactsEndpoint();
        app.MapGetContactEndpoint();
        app.MapUpdateContactEndpoint();
        app.MapDeleteContactEndpoint();
        app.UseSwagger();
        app.UseSwaggerUI();
        
        using IServiceScope scope = app.Services.CreateScope();
        ContactBookContext context = scope.ServiceProvider.GetRequiredService<ContactBookContext>();
        context.Database.Migrate();
        return app;
    }
}