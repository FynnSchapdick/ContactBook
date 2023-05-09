namespace ContactBook.Api.Endpoints.GetContacts;

public sealed record GetContractsRequest(int Page, int PageSize = 10, string? SearchFilter = null);