﻿using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

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
    });

    return services;
  }
}
