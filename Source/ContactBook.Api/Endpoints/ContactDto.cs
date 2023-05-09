namespace ContactBook.Api.Endpoints;

public sealed record ContactDto(Guid ContactId, string ContactName, string? ContactEmail, string? ContactMobile);