using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace QuizMaker.Common.Infrastructure.Logging;

[SuppressMessage("Minor Code Smell", "S6672:Generic logger injection should match enclosing type")]
[SuppressMessage("Usage", "CA2254:Template should be a static expression")]
public class LoggerAdaptor<TType> : ILoggerAdaptor<TType>
{
  private readonly ILogger<TType> _logger;

  public LoggerAdaptor(ILogger<TType> logger)
  {
    _logger = logger;
  }
  
  public void LogInformation(string? message, params object?[] args)
  {
    _logger.LogInformation(message,args);
  }

  public void LogWarning(string? message, params object?[] args)
  {
    _logger.LogWarning(message, args);
  }

  public void LogError(Exception? exception, string? message, params object?[] args)
  {
    _logger.LogError(exception, message,args);
  }

  public void LogCustom(long elapsedTime, TimeOnly currentTime, string className)
  {
    _logger.LogCustomInformation(elapsedTime, currentTime, className);
  }
}