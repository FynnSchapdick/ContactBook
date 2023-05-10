using System.Net;
using ContactBook.Api.Data;
using ContactBook.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace ContactBook.Api.Endpoints.GetContacts;

public static class GetContactsEndpoint
{
    private const string GetContactsRoute = "contacts";
    private const string ContactsTag = "Contacts";

    public static void MapGetContactsEndpoint(this WebApplication app)
    {
        app.MapGet(GetContactsRoute, GetContacts)
            .Produces<PaginatedContactsResponse>()
            .Produces((int) HttpStatusCode.InternalServerError)
            .AddEndpointFilter<ValidatorFilter<GetContactsRequest>>()
            .WithTags(ContactsTag);
    }

    private static async Task<IResult> GetContacts([AsParameters] GetContactsRequest request, ContactBookContext dbContext, CancellationToken cancellationToken = default)
    {
        try
        {
            int skip = (request.Page - 1) * request.PageSize;
            int totalCount = await dbContext.Contacts.CountAsync(cancellationToken);
            int totalPages = (int) Math.Ceiling((double) totalCount / request.PageSize);

            IQueryable<Contact> query = dbContext.Contacts.AsQueryable();
            if (!string.IsNullOrEmpty(request.SearchFilter))
            {
                query = query.Where(c => EF.Functions.Like(c.Name, $"%{request.SearchFilter}%")
                                         || c.Email != null && EF.Functions.Like(c.Email, $"%{request.SearchFilter}%")
                                         || c.Mobile != null && EF.Functions.Like(c.Mobile, $"%{request.SearchFilter}%"));
            }

            List<ContactDto> contactDtos = await query.Skip(skip).Take(request.PageSize).Select(x => new ContactDto(x.Id, x.Name, x.Email, x.Mobile)).ToListAsync(cancellationToken);
            PaginatedContactsResponse response = new PaginatedContactsResponse(contactDtos, request.Page, request.PageSize, totalCount, totalPages);
            return Results.Ok(response);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message, statusCode: (int) HttpStatusCode.InternalServerError);
        }
    }
}