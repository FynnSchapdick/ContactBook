using ContactBook.Api.Data;
using ContactBook.Api.Endpoints.CreateContact;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Exceptions;

namespace ContactBook.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddApi(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithMachineName()
                .WriteTo.Console();
        });
        builder.Services.AddDbContext<ContactBookContext>(options =>
        {
            options.UseSqlite(builder.Configuration.GetConnectionString("ContactBook"));
            options.UseSnakeCaseNamingConvention();
        });
        builder.Services.AddValidatorsFromAssemblyContaining<CreateContactRequestValidator>();
        builder.Services.AddSwaggerGen();
        builder.Services.AddEndpointsApiExplorer();
        return builder;
    }
}