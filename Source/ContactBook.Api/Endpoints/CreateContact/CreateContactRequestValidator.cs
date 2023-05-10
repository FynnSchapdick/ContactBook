using FluentValidation;
using PhoneNumbers;

namespace ContactBook.Api.Endpoints.CreateContact;

public sealed class CreateContactRequestValidator : AbstractValidator<CreateContactRequest>
{
    private const string MobileErrorMessage = "Mobile number does not correspond to E.164 format";
    
    public CreateContactRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(75);
        
        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.Email));
        
        RuleFor(x => x.Mobile)
            .Must(x => x != null && PhoneNumber.TryParse(x, out PhoneNumber _))
            .When(x =>  !string.IsNullOrEmpty(x.Mobile))
            .WithMessage(MobileErrorMessage);
    }
}