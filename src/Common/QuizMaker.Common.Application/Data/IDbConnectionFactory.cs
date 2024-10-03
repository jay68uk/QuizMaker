using System.Data.Common;

namespace QuizMaker.Common.Application.Data;

public interface IDbConnectionFactory
{
  ValueTask<DbConnection?> OpenConnectionAsync();
}