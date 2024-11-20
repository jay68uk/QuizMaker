using System.Net.Http.Json;

namespace QuizUser.Features.Identity;

public sealed class KeyCloakClient(HttpClient httpClient)
{
  public async Task<string> RegisterUser(UserRepresentation user, CancellationToken cancellationToken = default)
  {
    var httpResponseMessage = await httpClient.PostAsJsonAsync(
      "users",
      user,
      cancellationToken);

    httpResponseMessage.EnsureSuccessStatusCode();

    return ExtractIdentityIdFromLocationHeader(httpResponseMessage);
  }

  public async Task UpdateUser(UserRepresentation user, CancellationToken cancellationToken = default)
  {
    var httpResponseMessage = await httpClient.PutAsJsonAsync(
      "users/" + user.Id,
      user,
      cancellationToken);

    httpResponseMessage.EnsureSuccessStatusCode();
  }

  private static string ExtractIdentityIdFromLocationHeader(
    HttpResponseMessage httpResponseMessage)
  {
    const string usersSegmentName = "users/";

    var locationHeader = httpResponseMessage.Headers.Location?.PathAndQuery;

    if (locationHeader is null)
    {
      throw new InvalidOperationException("Location header is null");
    }

    var userSegmentValueIndex = locationHeader.IndexOf(
      usersSegmentName,
      StringComparison.InvariantCultureIgnoreCase);

    var identityId = locationHeader[(userSegmentValueIndex + usersSegmentName.Length)..];

    return identityId;
  }
}