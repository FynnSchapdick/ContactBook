using System.Net;
using ContactBook.Api.Data;
using ContactBook.Api.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactBook.Api.Endpoints.UpdateContact;

public static class UpdateContactEndpoint
{
    private const string UpdateContactRoute = "/contacts/{contactId:guid}";
    private const string UpdateContactContentType = "application/json";
    private const string ContactsTag = "Contacts";
    
    public static void MapUpdateContactEndpoint(this WebApplication app)
    {
        app.MapPut(UpdateContactRoute, UpdateContact)
            .Accepts<UpdateContactRequest>(UpdateContactContentType)
            .Produces((int) HttpStatusCode.OK)
            .Produces((int) HttpStatusCode.Conflict)
            .Produces((int) HttpStatusCode.InternalServerError)
            .AddEndpointFilter<ValidatorFilter<UpdateContactRequest>>()
            .WithTags(ContactsTag);
    }

    private static async Task<IResult> UpdateContact([FromRoute] Guid contactId, [FromBody] UpdateContactRequest request, ContactBookContext dbContext, CancellationToken cancellationToken = default)
    {
        try
        {
            Contact? contact = await dbContext.Contacts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == contactId, cancellationToken);
            if (contact is null)
            {
                return Results.NotFound(contactId);
            }

            contact = contact with
            {
                Name = request.Name,
                Email = request.Email,
                Mobile = request.Mobile
            };

            dbContext.Contacts.Update(contact);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Results.Ok();
        }
        catch (DbUpdateException dbUpdateException)
        {
            return Results.Conflict(dbUpdateException.InnerException?.Message);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message, statusCode: (int) HttpStatusCode.InternalServerError);
        }
    }
}