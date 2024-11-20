using QuizUser.IntegrationTests.TestSetup;

namespace QuizUser.IntegrationTests;

public class GetUserTests : BaseIntegrationTest
{
  public GetUserTests(IntegrationTestWebAppFactory factory) : base(factory)
  {
  }

  [Fact]
  public async Task CanGetUser()
  {
    Assert.True(true);
  }
}