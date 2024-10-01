using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using QuizMaker.Common.Application.Data;

namespace QuizMaker.Common.Infrastructure;

public static class InfrastructureConfiguration
{
  public static IServiceCollection AddInfrastructure(this IServiceCollection services)
  {
    services.TryAddScoped<IDbConnectionFactory, DbConnectionFactory>();
    
    return services;
  }
}