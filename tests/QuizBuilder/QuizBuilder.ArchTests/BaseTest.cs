using System.Reflection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using QuizMaker.Common.Domain;

namespace QuizBuilder.ArchTests;

public abstract class BaseTest
{
    protected static readonly Assembly DomainAssembly = typeof(Entity).Assembly;

    protected static readonly Assembly PresentationAssembly = typeof(Program).Assembly;
}
