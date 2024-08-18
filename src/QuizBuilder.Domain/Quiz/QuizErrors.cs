namespace QuizBuilder.Domain.Quiz;

public static class QuizErrors
{
  public const string PrepareError = "Current Status is incorrect for allowing quiz preparation! status must be Draft or Completed.";

  public const string PrepareAlreadyDoneError = "Quiz is already prepared and codes generated!";

  public const string CompletedError = "Quiz must have Running status before it can be completed!";

  public const string StartError = "Quiz must have Ready status before it can be started!";
}