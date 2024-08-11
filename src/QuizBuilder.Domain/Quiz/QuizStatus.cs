namespace QuizMaker.Domain.Quiz;

public enum QuizStatus
{
  Draft = 1,
  Ready = 2,
  Running = 4,
  Completed = 8,
  Deleted = 16
}