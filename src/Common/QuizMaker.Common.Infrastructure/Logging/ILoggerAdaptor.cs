using System.Diagnostics.CodeAnalysis;

namespace QuizMaker.Common.Infrastructure.Logging;

[SuppressMessage("Major Code Smell", "S2326:Unused type parameters should be removed")]
public interface ILoggerAdaptor<TType>
{
  void LogInformation(string? message, params object?[] args);

  void LogWarning(string? message, params object?[] args);

  void LogError(Exception? exception, string? message, params object?[] args);

  void LogCustom(long elapsedTime, TimeOnly currentTime,
    string className);
}