namespace ContactBook.Api.Endpoints.CreateContact;

public sealed record CreateContactRequest(string Name, string? Email = null, string? Mobile = null);