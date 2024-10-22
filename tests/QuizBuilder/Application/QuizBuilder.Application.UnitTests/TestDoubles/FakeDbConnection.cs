using System.Data;
using System.Data.Common;
using QuizBuilder.Application.Features.GetQuizById;

namespace QuizBuilder.Application.UnitTests.TestDoubles;

public class FakeDbConnection : DbConnection
{
    private readonly Func<string, object, IEnumerable<QuizResponse>> _queryAsyncHandler;

    public FakeDbConnection(Func<string, object, IEnumerable<QuizResponse>> queryAsyncHandler)
    {
        _queryAsyncHandler = queryAsyncHandler;
    }

    public override string ConnectionString { get; set; }
    public override string Database => "FakeDatabase";
    public override string DataSource => "FakeDataSource";
    public override string ServerVersion => "1.0";
    public override ConnectionState State => ConnectionState.Open;

    public override void ChangeDatabase(string databaseName) { }
    public override void Close() { }
    public override void Open() { }

    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel) => null;

    protected override DbCommand CreateDbCommand() => null;

    public Task<IEnumerable<QuizResponse>> QueryAsync(string sql, object parameters)
    {
        return Task.FromResult(_queryAsyncHandler(sql, parameters));
    }
}
