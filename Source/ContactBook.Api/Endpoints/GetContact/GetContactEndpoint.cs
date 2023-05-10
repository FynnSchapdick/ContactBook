using System.Net;
using ContactBook.Api.Data;
using ContactBook.Api.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactBook.Api.Endpoints.GetContact;

public static class GetContactEndpoint
{
    private const string GetContactRoute = "contacts/{contactId:guid}";
    internal const string RouteName = "GetContact";
    private const string ContactsTag = "Contacts";
    
    public static void MapGetContactEndpoint(this WebApplication app)
    {
        app.MapGet(GetContactRoute, GetContact)
            .Produces<ContactDto>()
            .Produces((int) HttpStatusCode.NotFound)
            .Produces((int) HttpStatusCode.InternalServerError)
            .WithName(RouteName)
            .WithTags(ContactsTag);
    }

    private static async Task<IResult> GetContact([FromRoute] Guid contactId, ContactBookContext dbContext, CancellationToken cancellationToken = default)
    {
        try
        {
            Contact? contact = await dbContext.Contacts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == contactId, cancellationToken);
            if (contact is null)
            {
                return Results.NotFound(contactId);
            }
            
            return Results.Ok(new ContactDto(contact.Id, contact.Name, contact.Email, contact.Mobile));
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }
}