using System.Reflection;
using FluentAssertions;
using NetArchTest.Rules;
using QuizMaker.Common.Domain;
using Xunit.Abstractions;

namespace QuizBuilder.ArchTests.Domain;

public class CodeStructureTests(ITestOutputHelper testOutputHelper) : BaseTest
{
  [Fact]
  public void DomainEvents_Should_BeSealed()
  {
    var result = Types.InAssembly(DomainAssembly)
      .That()
      .ImplementInterface(typeof(IDomainEvent))
      .Should()
      .BeSealed().Or().BeAbstract()
      .GetResult();

    OutputFailedTestItem(result);

    result.IsSuccessful.Should().BeTrue();
  }

  [Fact]
  public void DomainEvents_Should_HaveDomainEventsPostfix()
  {
    var result = Types.InAssembly(DomainAssembly)
      .That()
      .ImplementInterface(typeof(IDomainEvent))
      .Should()
      .HaveNameEndingWith("DomainEvent")
      .GetResult();

    OutputFailedTestItem(result);

    result.IsSuccessful.Should().BeTrue();
  }

  private void OutputFailedTestItem(TestResult result)
  {
    if (!result.IsSuccessful)
    {
      var failingClasses = result.FailingTypes
        .Select(detail => detail.FullName)
        .ToList();

      foreach (var failingClass in failingClasses)
      {
        testOutputHelper.WriteLine($"Class failed rule: {failingClass}");
      }
    }
  }

  [Fact]
  public void Entities_Should_NotHavePublicConstructor()
  {
    var entityTypes = Types.InAssembly(DomainAssembly)
      .That()
      .Inherit(typeof(Entity))
      .GetTypes()
      .Select(t => t.ReflectionType)
      .ToList();

    var failingTypes =
      (from entityType in entityTypes
        let constructors =
          entityType.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
        where constructors.Any(c => c.IsPublic)
        select entityType).ToList();

    failingTypes.Should().BeEmpty();
  }
}