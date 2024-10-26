using System.Reflection;
using QuizBuilder.Infrastructure;
using QuizBuilder.Infrastructure.SeedData;
using QuizMaker.Api;
using QuizMaker.Api.Extensions;
using QuizMaker.Common.Application;
using QuizMaker.Common.Infrastructure;
using QuizMaker.Common.Presentation;
using QuizUser;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

Assembly[] moduleApplicationAssemblies =
[
    QuizBuilder.Application.AssemblyReference.Assembly,
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

app.UseExceptionHandler();

app.UseEndpoints();

if (app.Environment.IsDevelopment())
{
    app.InitialiseQuizSeeding();
    app.MapScalarApiReference();
}

app.Run();
