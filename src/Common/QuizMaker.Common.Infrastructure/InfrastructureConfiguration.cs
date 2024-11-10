using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using QuizMaker.Common.Application.Data;
using QuizMaker.Common.Infrastructure.Authorisation;

namespace QuizMaker.Common.Infrastructure;

public static class InfrastructureConfiguration
{
  public static IServiceCollection AddInfrastructure(this IServiceCollection services, string databaseConnectionString)
  {
    var npgsqlDataSource = new NpgsqlDataSourceBuilder(databaseConnectionString).Build();
    services.TryAddSingleton(npgsqlDataSource);

    services.TryAddScoped<IDbConnectionFactory, DbConnectionFactory>();

    services.AddAuthorisationPolicies();

    return services;
  }
}