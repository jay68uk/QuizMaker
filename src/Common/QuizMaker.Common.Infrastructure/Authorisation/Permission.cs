namespace QuizMaker.Common.Infrastructure.Authorisation;

public sealed class Permission
{
  public static readonly Permission GetUser = new("users:read");
  public static readonly Permission ModifyUser = new("users:update");
  public static readonly Permission GetQuizzes = new("quizzes:read");
  public static readonly Permission RunQuizzes = new("quizzes:run");
  public static readonly Permission ModifyQuizzes = new("quizzes:update");

  public Permission(string code)
  {
    Code = code;
  }

  public string Code { get; }
}