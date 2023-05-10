using FluentValidation;

namespace ContactBook.Api.Endpoints.GetContacts;

public sealed class GetContactsRequestValidator : AbstractValidator<GetContactsRequest>
{
    public GetContactsRequestValidator()
    {
        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);
    }
}