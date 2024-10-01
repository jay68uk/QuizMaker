using System.Reflection;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace QuizMaker.Common.Presentation;

public static class CommonPresentationModule
{
  public static IServiceCollection AddEndpoints(this IServiceCollection services, params Assembly[] assemblies )
  {
    services.AddFastEndpoints(options => options.Assemblies = assemblies)
      .SwaggerDocument();
    
    return services;
  }

  public static IApplicationBuilder UseEndpoints(this IApplicationBuilder app)
  {
    app.UseFastEndpoints();
    
    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
    {
      app.UseSwaggerGen();
    }
    
    
    return app;
  }
}