using QuizBuilder.Infrastructure;
using QuizBuilder.Infrastructure.SeedData;
using QuizMaker.Api;
using QuizMaker.Common.Application;
using QuizMaker.Common.Infrastructure;
using QuizMaker.Common.Presentation;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpoints(ModuleAssemblyReferences.PresentationAssemblies());
builder.Services.AddApplication(ModuleAssemblyReferences.ApplicationAssemblies());

var databaseConnectionString = builder.Configuration.GetConnectionStringOrThrow("Database");
builder.Services.AddInfrastructure(databaseConnectionString);

builder.Services.AddQuizBuilderServices(databaseConnectionString);

var app = builder.Build();

app.UseEndpoints();

if (app.Environment.IsDevelopment())
{
    app.InitialiseQuizSeeding();
    app.MapScalarApiReference();
}

app.Run();
