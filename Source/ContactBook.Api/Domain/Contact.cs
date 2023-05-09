using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using MassTransit;

namespace ContactBook.Api.Domain;

public sealed record Contact
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    
    [EmailAddress]
    public string? Email { get; init; }
    public string? Mobile { get; init; }

    private Contact(string? email, string? mobile)
    {
        Email = email;
        Mobile = mobile;
    }

    public static Contact CreateNew(string name, string? email, string? mobile)
    {
        if (name.Length < 3)
        {
            throw new UnreachableException();
        }

        return new Contact(email, mobile)
        {
            Id = NewId.NextGuid(),
            Name = name
        };
    }
}