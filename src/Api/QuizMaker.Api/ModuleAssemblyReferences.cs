using System.Reflection;

namespace QuizMaker.Api;

public static class ModuleAssemblyReferences
{
  public static Assembly[] PresentationAssemblies() => new[]
  {
    QuizBuilder.Presentation.AssemblyReference.Assembly
  };

  public static Assembly[] ApplicationAssemblies() => new[]
  {
    QuizBuilder.Application.AssemblyReference.Assembly
  };
}