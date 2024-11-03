using Microsoft.Extensions.Logging;

namespace QuizMaker.Common.Infrastructure.Logging;

public static partial class LoggerExtensions
{
  [LoggerMessage(LogLevel.Information, "Custom fast log message with params elapsed time: {ElapsedTime}ms, current time: {CurrentTime}, class name: {ClassName}")]
  public static partial void LogCustomInformation(this ILogger logger, long elapsedTime, TimeOnly currentTime,
    string className);
}