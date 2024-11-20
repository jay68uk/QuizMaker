using Ardalis.Result;
using FluentValidation;
using FluentValidation.Results;

namespace QuizMaker.Common.Application.Extensions;

public static class FluentValidationExtensions
{
  public static ValidationError[] ToArdalisValidationErrors(this ValidationFailure[]? validationResult)
  {
    var validationErrors = new List<ValidationError>();
    if (validationResult == null)
    {
      return [.. validationErrors];
    }

    validationErrors.AddRange(validationResult.Select(error =>
      new ValidationError(
        error.PropertyName,
        error.ErrorMessage,
        error.ErrorCode,
        error.Severity.ToArdalisSeverity())));

    return [.. validationErrors];
  }

  private static ValidationSeverity ToArdalisSeverity(this Severity severity)
  {
    return severity switch
    {
      Severity.Error => ValidationSeverity.Error,
      Severity.Warning => ValidationSeverity.Warning,
      Severity.Info => ValidationSeverity.Info,
      _ => throw new ArgumentOutOfRangeException(nameof(severity), severity, null)
    };
  }
}