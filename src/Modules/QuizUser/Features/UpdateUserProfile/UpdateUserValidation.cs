using FluentValidation;
using QuizUser.Abstractions.Errors;

namespace QuizUser.Features.UpdateUserProfile;

internal sealed class UpdateUserValidation : AbstractValidator<UpdateUserCommand>
{
  public UpdateUserValidation()
  {
    RuleFor(u => u.Email)
      .EmailAddress()
      .WithMessage(ValidationMessages.InvalidEmail);

    RuleFor(u => u.FirstName)
      .Length(ValidationMessages.MinNameLength, ValidationMessages.MaxNameLength)
      .WithMessage(ValidationMessages.InvalidNameLength);

    RuleFor(u => u.LastName)
      .Length(ValidationMessages.MinNameLength, ValidationMessages.MaxNameLength)
      .WithMessage(ValidationMessages.InvalidNameLength);
  }
}