using System.Data.Common;
using Npgsql;
using QuizMaker.Common.Application.Data;

namespace QuizMaker.Common.Infrastructure;

internal sealed class DbConnectionFactory(NpgsqlDataSource dataSource) : IDbConnectionFactory
{
  public async ValueTask<DbConnection?> OpenConnectionAsync()
  {
      return await dataSource.OpenConnectionAsync();
  }
}
