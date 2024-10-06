using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuizBuilder.Infrastructure.Data;

namespace QuizBuilder.Infrastructure;

public static class QuizBuilderModule
{
    public static IServiceCollection AddQuizBuilderServices(this IServiceCollection services, string databaseConnectionString)
    {
        services.AddDbContext<QuizDbContext>(options =>
            options.UseNpgsql(databaseConnectionString).UseSnakeCaseNamingConvention());
        
        return services;
    }
  
}
