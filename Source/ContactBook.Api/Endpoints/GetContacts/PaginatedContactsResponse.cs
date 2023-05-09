namespace ContactBook.Api.Endpoints.GetContacts;

public sealed record PaginatedContactsResponse(List<ContactDto> Contacts, int Page, int PageSize, int TotalCount, int TotalPages);