using Ardalis.Result;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using QuizMaker.Common.Application.Extensions;
using QuizMaker.Common.Application.Messaging;

namespace QuizMaker.Common.Application.Behaviours;

internal sealed class ValidationPipelineBehavior<TRequest, TResponse>(
  IEnumerable<IValidator<TRequest>> validators)
  : IPipelineBehavior<TRequest, TResponse>
  where TRequest : IBaseCommand
{
  public async Task<TResponse> Handle(
    TRequest request,
    RequestHandlerDelegate<TResponse> next,
    CancellationToken cancellationToken)
  {
    var validationFailures = await Validate(request);

    if (validationFailures.Length == 0)
    {
      return await next();
    }

    if (typeof(TResponse) == typeof(Result))
    {
      return (TResponse)(object)Result.Invalid(validationFailures.ToArdalisValidationErrors());
    }

    throw new ValidationException(validationFailures);
  }

  private async Task<ValidationFailure[]> Validate(TRequest request)
  {
    if (!validators.Any())
    {
      return [];
    }

    var context = new ValidationContext<TRequest>(request);

    var validationResults = await Task.WhenAll(
      validators.Select(validator => validator.ValidateAsync(context)));

    var validationFailures = validationResults
      .Where(validationResult => !validationResult.IsValid)
      .SelectMany(validationResult => validationResult.Errors)
      .ToArray();

    return validationFailures;
  }
}