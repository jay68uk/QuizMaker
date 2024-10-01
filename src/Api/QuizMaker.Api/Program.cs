using QuizMaker.Api;
using QuizMaker.Common.Application;
using QuizMaker.Common.Infrastructure;
using QuizMaker.Common.Presentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpoints(ModuleAssemblyReferences.PresentationAssemblies());
builder.Services.AddApplication(ModuleAssemblyReferences.ApplicationAssemblies());

builder.Services.AddInfrastructure();

var app = builder.Build();

app.UseEndpoints();

app.UseHttpsRedirection();

app.Run();
