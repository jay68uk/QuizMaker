using System.Reflection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using QuizMaker.SharedKernel;

namespace QuizBuilder.ArchTests;

public abstract class BaseTest
{
    //protected static readonly Assembly ApplicationAssembly = typeof(IBaseCommand).Assembly;

    protected static readonly Assembly DomainAssembly = typeof(Entity).Assembly;

    //protected static readonly Assembly InfrastructureAssembly = typeof(ApplicationDbContext).Assembly;

    protected static readonly Assembly PresentationAssembly = typeof(Program).Assembly;
}
