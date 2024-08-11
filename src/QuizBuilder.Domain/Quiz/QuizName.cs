using Ardalis.GuardClauses;

namespace QuizMaker.Domain.Quiz;

public sealed record QuizName
{
  public string Value { get; private set; }
   
  public QuizName(string? value)
  {
    Guard.Against.NullOrEmpty(value);

    Value = value;
  }
}