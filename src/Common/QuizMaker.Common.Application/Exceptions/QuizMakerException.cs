using QuizMaker.Common.Domain;

namespace QuizMaker.Common.Application.Exceptions;

public sealed class QuizMakerException : Exception
{
  public QuizMakerException(string requestName, Error? error = default, Exception? innerException = default)
    : base("Application exception", innerException)
  {
    RequestName = requestName;
    Error = error;
  }

  public string RequestName { get; }

  public Error? Error { get; }
}