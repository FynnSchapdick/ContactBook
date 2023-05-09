using System.Net;
using ContactBook.Api.Data;
using ContactBook.Api.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ContactBook.Api.Endpoints.GetContact;

public static class GetContactEndpoint
{
    private const string GetContactRoute = "contacts/{contactId:guid}";
    internal const string RouteName = "GetContact";
    
    public static void MapGetContactEndpoint(this WebApplication app)
    {
        app.MapGet(GetContactRoute, GetContact)
            .Produces<ContactDto>()
            .Produces((int) HttpStatusCode.NotFound)
            .Produces((int) HttpStatusCode.InternalServerError)
            .WithName(RouteName);
    }

    private static async Task<IResult> GetContact([FromRoute] Guid contactId, ContactBookContext dbContext, CancellationToken cancellationToken = default)
    {
        try
        {
            Contact? contact = await dbContext.Contacts.FindAsync(new object?[]{ contactId },cancellationToken: cancellationToken);
            if (contact is null)
            {
                return Results.NotFound(contactId);
            }

            ContactDto dto = new ContactDto(contact.Id, contact.Name, contact.Email, contact.Mobile);
            return Results.Ok(dto);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }
}