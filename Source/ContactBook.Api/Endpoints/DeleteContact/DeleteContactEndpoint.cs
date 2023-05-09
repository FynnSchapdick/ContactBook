using System.Net;
using ContactBook.Api.Data;
using ContactBook.Api.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ContactBook.Api.Endpoints.DeleteContact;

public static class DeleteContactEndpoint
{
    private const string DeleteContactRoute = "/contacts/{contactId:guid}";

    public static void MapDeleteContactEndpoint(this WebApplication app)
    {
        app.MapDelete(DeleteContactRoute, DeleteContact)
            .Produces((int) HttpStatusCode.OK)
            .Produces((int) HttpStatusCode.NotFound)
            .Produces((int) HttpStatusCode.Conflict)
            .Produces((int) HttpStatusCode.InternalServerError);
    }

    private static async Task<IResult> DeleteContact([FromRoute] Guid contactId, ContactBookContext dbContext,
        CancellationToken cancellationToken = default)
    {
        try
        {
            Contact? contact = await dbContext.Contacts.FindAsync(new object?[] {contactId}, cancellationToken: cancellationToken);
            if (contact is null)
            {
                return Results.NotFound(contactId);
            }

            dbContext.Contacts.Remove(contact);
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