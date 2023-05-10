namespace ContactBook.Api.Endpoints.UpdateContact;

public sealed record UpdateContactRequest(string Name, string? Email = null, string? Mobile = null);