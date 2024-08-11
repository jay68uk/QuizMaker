using Ardalis.GuardClauses;

namespace QuizMaker.Domain.Question;

public record QuestionNumber
{
  public int Value { get; private set; }
   
  public QuestionNumber(int value)
  {
    Guard.Against.NegativeOrZero(value);

    Value = value;
  }
}