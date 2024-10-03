using System.Data.Common;
using QuizMaker.Common.Application.Data;

namespace QuizMaker.Common.Infrastructure;

internal sealed class DbConnectionFactory() : IDbConnectionFactory
{
  public async ValueTask<DbConnection?> OpenConnectionAsync()
  {
    return await Task.FromResult<DbConnection>(null!);
  }
}
