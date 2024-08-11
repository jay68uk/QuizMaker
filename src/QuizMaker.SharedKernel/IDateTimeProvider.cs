namespace QuizMaker.SharedKernel;

public interface IDateTimeProvider
{
  DateTime UtcNow { get; }
}