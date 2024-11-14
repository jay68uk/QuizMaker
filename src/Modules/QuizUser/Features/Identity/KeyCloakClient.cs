using System.Net.Http.Json;

namespace QuizUser.Features.Identity;

internal sealed class KeyCloakClient(HttpClient httpClient)
{
  internal async Task<string> RegisterUser(UserRepresentation user, CancellationToken cancellationToken = default)
  {
    // ReSharper disable once SuggestVarOrType_SimpleTypes
#pragma warning disable IDE0007
    HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync(
#pragma warning restore IDE0007
      "users",
      user,
      cancellationToken);

    httpResponseMessage.EnsureSuccessStatusCode();

    return ExtractIdentityIdFromLocationHeader(httpResponseMessage);
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

    var identityId = locationHeader.Substring(userSegmentValueIndex + usersSegmentName.Length);

    return identityId;
  }
}