using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using QuizUser.Abstractions.Authorisation;
using QuizUser.Abstractions.Identity;
using QuizUser.Abstractions.Users;
using QuizUser.Features.Authentication;
using QuizUser.Features.Authorisation;
using QuizUser.Features.Identity;
using QuizUser.Features.Users.Data;
using IUnitOfWork = QuizUser.Abstractions.Users.IUnitOfWork;

namespace QuizUser;

[SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out")]
public static class UsersModule
{
  public static IServiceCollection AddUsersModule(
    this IServiceCollection services,
    IConfiguration configuration)
  {
    // services.AddDomainEventHandlers();
    //
    // services.AddIntegrationEventHandlers();

    services.AddInfrastructure(configuration);

    return services;
  }

  private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddAuthenticationInternal();

    services.AddAuthorizationInternal();

    services.AddScoped<IPermissionService, PermissionService>();

    services.Configure<KeyCloakOptions>(configuration.GetSection("Users:KeyCloak"));

    services.AddTransient<KeyCloakAuthDelegatingHandler>();

    services
      .AddHttpClient<KeyCloakClient>((serviceProvider, httpClient) =>
      {
        var keycloakOptions = serviceProvider
          .GetRequiredService<IOptions<KeyCloakOptions>>().Value;

        httpClient.BaseAddress = new Uri(keycloakOptions.AdminUrl);
      })
      .AddHttpMessageHandler<KeyCloakAuthDelegatingHandler>();

    services.AddTransient<IIdentityProviderService, IdentityProviderService>();

    services.AddDbContext<UsersDbContext>((sp, options) =>
      options
        .UseNpgsql(
          configuration.GetConnectionString("Database"),
          npgsqlOptions => npgsqlOptions
            .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Users))
        //.AddInterceptors(sp.GetRequiredService<InsertOutboxMessagesInterceptor>())
        .UseSnakeCaseNamingConvention());

    services.AddScoped<IUserRepository, UserRepository>();

    services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UsersDbContext>());

    // services.Configure<OutboxOptions>(configuration.GetSection("Users:Outbox"));

    // services.ConfigureOptions<ConfigureProcessOutboxJob>();

    // services.Configure<InboxOptions>(configuration.GetSection("Users:Inbox"));

    // services.ConfigureOptions<ConfigureProcessInboxJob>();
  }

  // private static void AddDomainEventHandlers(this IServiceCollection services)
  // {
  //     Type[] domainEventHandlers = MediaTypeNames.Application.AssemblyReference.Assembly
  //         .GetTypes()
  //         .Where(t => t.IsAssignableTo(typeof(IDomainEventHandler)))
  //         .ToArray();
  //
  //     foreach (var domainEventHandler in domainEventHandlers)
  //     {
  //         services.TryAddScoped(domainEventHandler);
  //
  //         var domainEvent = domainEventHandler
  //             .GetInterfaces()
  //             .Single(i => i.IsGenericType)
  //             .GetGenericArguments()
  //             .Single();
  //
  //         var closedIdempotentHandler = typeof(IdempotentDomainEventHandler<>).MakeGenericType(domainEvent);
  //
  //         services.Decorate(domainEventHandler, closedIdempotentHandler);
  //     }
  // }
  //
  // private static void AddIntegrationEventHandlers(this IServiceCollection services)
  // {
  //     Type[] integrationEventHandlers = Presentation.AssemblyReference.Assembly
  //         .GetTypes()
  //         .Where(t => t.IsAssignableTo(typeof(IIntegrationEventHandler)))
  //         .ToArray();
  //
  //     foreach (var integrationEventHandler in integrationEventHandlers)
  //     {
  //         services.TryAddScoped(integrationEventHandler);
  //
  //         var integrationEvent = integrationEventHandler
  //             .GetInterfaces()
  //             .Single(i => i.IsGenericType)
  //             .GetGenericArguments()
  //             .Single();
  //
  //         var closedIdempotentHandler =
  //             typeof(IdempotentIntegrationEventHandler<>).MakeGenericType(integrationEvent);
  //
  //         services.Decorate(integrationEventHandler, closedIdempotentHandler);
  //     }
  // }

  public static IApplicationBuilder UseAuthenticationandAuthorisation(this IApplicationBuilder app)
  {
    app.UseAuthentication();
    app.UseAuthorization();

    return app;
  }
}