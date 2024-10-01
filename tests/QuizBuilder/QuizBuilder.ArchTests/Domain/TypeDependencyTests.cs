using FluentAssertions;
using NetArchTest.Rules;

namespace QuizBuilder.ArchTests.Domain;

public class TypeDependencyTests : BaseTest
{
  [Fact]
  public void DomainType_Should_NotReferencePresentation()
  {
    var domainTypes = Types.InAssembly(DomainAssembly)
      .Should()
      .NotHaveDependencyOnAny(["QuizBuilder.Infrastructure.*",
        "QuizBuilder.Features",
        "QuizBuilder.Presentation.*"])
      .GetResult();

    domainTypes.IsSuccessful.Should().BeTrue();
  }
}