using System.Reflection;
using QuizBuilder.Infrastructure;
using QuizBuilder.Infrastructure.SeedData;
using QuizMaker.Api.Extensions;
using QuizMaker.Common.Application;
using QuizMaker.Common.Infrastructure;
using QuizMaker.Common.Infrastructure.Logging;
using QuizMaker.Common.Presentation;
using QuizUser;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using AssemblyReference = QuizBuilder.Application.AssemblyReference;

Log.Logger = new LoggerConfiguration()
  .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
  .Enrich.FromLogContext()
  .WriteTo.Console(new JsonFormatter())
  .CreateBootstrapLogger();

Log.Logger.Information("Starting QuizMaker");
try
{
  var builder = WebApplication.CreateBuilder(args);

  Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

  builder.Services.AddTransient(typeof(ILoggerAdaptor<>), typeof(LoggerAdaptor<>));
  builder.Host.UseSerilog();
  builder.Services.AddSingleton(Log.Logger);

  Assembly[] moduleApplicationAssemblies =
  [
    AssemblyReference.Assembly,
    QuizUser.AssemblyReference.Assembly
  ];

  Assembly[] modulePresentationAssemblies =
  [
    QuizBuilder.Presentation.AssemblyReference.Assembly,
    QuizUser.AssemblyReference.Assembly
  ];

  builder.Configuration.AddModuleConfiguration(["users"]);

  builder.Services.AddEndpoints(modulePresentationAssemblies);
  builder.Services.AddApplication(moduleApplicationAssemblies);

  var databaseConnectionString = builder.Configuration.GetConnectionStringOrThrow("Database");
  builder.Services.AddInfrastructure(databaseConnectionString);

  builder.Services.AddQuizBuilderServices(databaseConnectionString);

  builder.Services.AddUsersModule(builder.Configuration);

  var app = builder.Build();

  app.UseAuthenticationandAuthorisation();
  app.UseEndpoints();

  if (app.Environment.IsDevelopment())
  {
    app.InitialiseQuizSeeding();
    app.MapScalarApiReference();
  }

  app.Run();
}
catch (Exception e)
{
  Log.Fatal(e, "Application failed to start due to builder or app error!");
}
finally
{
  Log.CloseAndFlush();
}