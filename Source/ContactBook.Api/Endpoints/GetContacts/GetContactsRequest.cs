namespace ContactBook.Api.Endpoints.GetContacts;

public sealed record GetContactsRequest(int Page, int PageSize = 10, string? SearchFilter = null);