using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using QuizUser.Features.Identity;
using QuizUser.Features.Users.Data;
using Testcontainers.Keycloak;
using Testcontainers.PostgreSql;

namespace QuizUser.IntegrationTests.TestSetup;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
  private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
    .WithImage("postgres:latest")
    .WithDatabase("quizmaker")
    .WithUsername("postgres")
    .WithPassword("postgres")
    .WithWaitStrategy(Wait.ForUnixContainer()
      .UntilPortIsAvailable(5432)
      .UntilMessageIsLogged("database system is ready to accept connections"))
    .Build();

  private readonly KeycloakContainer _keycloakContainer = new KeycloakBuilder()
    .WithImage("quay.io/keycloak/keycloak:latest")
    .WithResourceMapping(
      new FileInfo("quizmaker-realm-export.json"),
      new FileInfo("/opt/keycloak/data/import/realm.json"))
    .WithCommand("--import-realm")
    .Build();

  public async Task InitializeAsync()
  {
    await _dbContainer.StartAsync();
    await _keycloakContainer.StartAsync();
    MigrateDatabase();
  }

  public new async Task DisposeAsync()
  {
    await _dbContainer.StopAsync();
    await _keycloakContainer.StopAsync();
  }

  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    Environment.SetEnvironmentVariable("ConnectionStrings:Database", _dbContainer.GetConnectionString());

    var keycloakAddress = _keycloakContainer.GetBaseAddress();
    var keyCloakRealmUrl = $"{keycloakAddress}realms/quizmaker";

    Environment.SetEnvironmentVariable(
      "Authentication:MetadataAddress",
      $"{keyCloakRealmUrl}/.well-known/openid-configuration");
    Environment.SetEnvironmentVariable(
      "Authentication:TokenValidationParameters:ValidIssuer",
      keyCloakRealmUrl);

    builder.ConfigureTestServices(services =>
    {
      services.Configure<KeyCloakOptions>(o =>
      {
        o.AdminUrl = $"{keycloakAddress}admin/realms/quizmaker/";
        o.TokenUrl = $"{keyCloakRealmUrl}/protocol/openid-connect/token";
      });
    });
  }

  private void MigrateDatabase()
  {
    var options = new DbContextOptionsBuilder<UsersDbContext>()
      .UseNpgsql(_dbContainer.GetConnectionString())
      .Options;

    using var context = new UsersDbContext(options);
    context.Database.Migrate();
  }

  private void SeedKeycloakUsers()
  {
    
  }
}