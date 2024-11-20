using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using QuizMaker.Common.Application.Behaviours;

namespace QuizMaker.Common.Application;

public static class ApplicationConfiguration
{
  public static IServiceCollection AddApplication(
    this IServiceCollection services,
    Assembly[] moduleAssemblies)
  {
    services.AddMediatR(config =>
    {
      config.RegisterServicesFromAssemblies(moduleAssemblies);
      config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
    });

    services.AddValidatorsFromAssemblies(moduleAssemblies, includeInternalTypes: true);
    return services;
  }
}