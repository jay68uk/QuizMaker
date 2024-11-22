using System.Net.Http.Json;
using FakeItEasy;
using FluentAssertions;
using QuizUser.Features.RegisterUser;
using QuizUser.IntegrationTests.TestSetup;

namespace QuizUser.IntegrationTests;

[Collection("IntegrationTestCollection")]
public class UserEndpointTests
{
  private readonly IntegrationTestWebAppFactory _appFixture;

  public UserEndpointTests(IntegrationTestWebAppFactory appFixture)
  {
    _appFixture = appFixture;
  }

  [Fact]
  public async Task Should_RegisterUser_WhenValidUserIsProvided()
  {
    var registrationRequest = A.Fake<RegisterUserRequest>();

    var httpResponse = await _appFixture.Client.PostAsJsonAsync("/users/register", registrationRequest);

    httpResponse.IsSuccessStatusCode.Should().BeTrue();
  }
}