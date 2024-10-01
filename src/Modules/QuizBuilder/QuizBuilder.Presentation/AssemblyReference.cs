using System.Reflection;

namespace QuizBuilder.Presentation;

public static class AssemblyReference
{
  public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}