namespace QuizUser.Abstractions.Errors;

internal static class ValidationMessages
{
  public const string InvalidEmail = "Invalid email address";
  public const int MaxNameLength = 30;
  public const int MinNameLength = 2;

  public static string InvalidNameLength => $"Name must be between {MinNameLength} and {MaxNameLength} characters";
}