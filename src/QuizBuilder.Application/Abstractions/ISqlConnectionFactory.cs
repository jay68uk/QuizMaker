using System.Data;

namespace QuizBuilder.Application.Abstractions;

public interface ISqlConnectionFactory
{
  IDbConnection CreateConnection();
}