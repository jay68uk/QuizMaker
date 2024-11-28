using System.Net.Http.Json;
using FastEndpoints.Testing;
using FluentAssertions;
using QuizUser.Features.RegisterUser;
using QuizUser.IntegrationTests.TestData;
using QuizUser.IntegrationTests.TestSetup;
using Xunit.Abstractions;

namespace QuizUser.IntegrationTests;

[Collection("IntegrationTestCollection")]
public class UserEndpointTests
{
  private readonly IntegrationTestWebAppFactory _appFixture;
  private readonly ITestOutputHelper _outputHelper;

  public UserEndpointTests(IntegrationTestWebAppFactory appFixture, ITestOutputHelper outputHelper)
  {
    _appFixture = appFixture;
    _outputHelper = outputHelper;
  }

  [Fact]
  [Priority(1)]
  public async Task Should_RegisterUser_WhenValidUserIsProvided()
  {
    var validUser = TestUsers.UserValid;
    var registrationRequest = new RegisterUserRequest(
      validUser["Email"],
      validUser["Password"],
      validUser["FirstName"],
      validUser["LastName"]);

    var httpResponse = await _appFixture.Client.PostAsJsonAsync("/users/register", registrationRequest);

    httpResponse.IsSuccessStatusCode.Should().BeTrue();
    httpResponse.Headers.Location!.AbsolutePath.Should().Contain("/users/profile");
    var body = await httpResponse.Content.ReadFromJsonAsync(typeof(string));
    _outputHelper.WriteLine($"Registered user:{body}");
    Guid.TryParse(body!.ToString(), out var userId);
    userId.Should().NotBe(Guid.Empty);
  }
}