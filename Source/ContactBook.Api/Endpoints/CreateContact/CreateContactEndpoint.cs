using System.Net;
using ContactBook.Api.Data;
using ContactBook.Api.Domain;
using ContactBook.Api.Endpoints.GetContact;
using Microsoft.EntityFrameworkCore;

namespace ContactBook.Api.Endpoints.CreateContact;

public static class CreateContactEndpoint
{
    private const string CreateContactRoute = "contacts";
    private const string CreateContactRequestContentType = "application/json";
    private const string ContactsTag = "Contacts";

    public static void MapCreateContactEndpoint(this WebApplication app)
    {
        app.MapPost(CreateContactRoute, CreateContact)
            .Accepts<CreateContactRequest>(CreateContactRequestContentType)
            .Produces((int) HttpStatusCode.Created)
            .Produces((int) HttpStatusCode.Conflict)
            .Produces((int) HttpStatusCode.BadRequest)
            .Produces((int) HttpStatusCode.InternalServerError)
            .AddEndpointFilter<ValidatorFilter<CreateContactRequest>>()
            .WithTags(ContactsTag);
    }

    private static async Task<IResult> CreateContact(CreateContactRequest request, ContactBookContext dbContext, CancellationToken cancellationToken = default)
    {
        try
        {
            Contact contact = Contact.CreateNew(request.Name, request.Email, request.Mobile);
            await dbContext.Contacts.AddAsync(contact, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Results.CreatedAtRoute(GetContactEndpoint.RouteName, new { ContactId = contact.Id });
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