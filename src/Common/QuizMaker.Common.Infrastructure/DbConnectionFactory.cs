using System.Data;
using System.Data.Common;
using Npgsql;
using QuizMaker.Common.Application.Data;
using QuizMaker.Common.Application.Exceptions;
using QuizMaker.Common.Domain;

namespace QuizMaker.Common.Infrastructure;

internal sealed class DbConnectionFactory(NpgsqlDataSource dataSource) : IDbConnectionFactory
{
  public async ValueTask<DbConnection> OpenConnection()
  {
    var connection = await dataSource.OpenConnectionAsync();

    if (connection is not { State: ConnectionState.Open })
    {
      throw new QuizMakerException(
        nameof(OpenConnection),
        Error.Failure("Postgres", "Could not establish a connection!"));
    }

    return connection;
  }
}