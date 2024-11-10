using FluentAssertions;
using NetArchTest.Rules;

namespace QuizBuilder.ArchTests.Domain;

public class TypeDependencyTests : BaseTest
{
  [Fact]
  public void PresentationType_Should_OnlyReferenceApplicationTypes()
  {
    var presentationTypes = Types.InAssembly(PresentationAssembly)
      .Should()
      .NotHaveDependencyOnAny("QuizBuilder.Infrastructure.*", "QuizBuilder.Domain.*")
      .GetResult();

    presentationTypes.IsSuccessful.Should().BeTrue();
  }

  [Fact]
  public void DomainType_Should_NotReferenceOtherTypes()
  {
    var domainTypes = Types.InAssembly(DomainAssembly)
      .Should()
      .NotHaveDependencyOnAny("QuizBuilder.Infrastructure.*", "QuizBuilder.Application.*", "QuizBuilder.Presentation.*")
      .GetResult();

    domainTypes.IsSuccessful.Should().BeTrue();
  }
}