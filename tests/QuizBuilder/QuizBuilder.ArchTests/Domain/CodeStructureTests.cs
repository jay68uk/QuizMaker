using System.Reflection;
using FluentAssertions;
using NetArchTest.Rules;
using QuizMaker.Common.Domain;

namespace QuizBuilder.ArchTests.Domain;

public class CodeStructureTests : BaseTest
{
  [Fact]
  public void DomainEvents_Should_BeSealed()
  {
   
    var result = Types.InAssembly(DomainAssembly)
      .That()
      .ImplementInterface(typeof(IDomainEvent))
      .Should()
      .BeSealed()
      .GetResult();

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

    result.IsSuccessful.Should().BeTrue();
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

    var failingTypes = new List<Type>();
    foreach (Type entityType in entityTypes)
    {
      ConstructorInfo[] constructors = entityType.GetConstructors(BindingFlags.Public |
                                                                  BindingFlags.Instance);

      if (constructors.Any(c => c.IsPublic))
      {
        failingTypes.Add(entityType);
      }
    }

    failingTypes.Should().BeEmpty();
  }
}