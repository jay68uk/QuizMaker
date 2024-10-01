using Ardalis.GuardClauses;

namespace QuizBuilder.Domain.Question;

public sealed record Description
{
  public string? Value { get; private set; }
   
  public Description(string? value)
  {
    Guard.Against.NullOrEmpty(value);

    Value = value;
  }
}