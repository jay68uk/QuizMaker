using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using QuizUser.Infrastructure.Authentication;

namespace QuizUser.Features.UserProfile;

internal sealed class GetUserProfile(ISender sender) : EndpointWithoutRequest
<Results<
  Ok<UserResponse>,
  NotFound,
  ProblemDetails>>
{
  public override void Configure()
  {
    Get("/users/profile");
  }

  public override async Task<Results<Ok<UserResponse>,
    NotFound,
    ProblemDetails>> ExecuteAsync(CancellationToken ct)
  {
    var userId = User.GetUserId();
    var query = new RequestUserByIdQuery(userId);

    var result = await sender.Send(query, ct);

    if (result.IsSuccess)
    {
      return TypedResults.Ok(new UserResponse(
        result.Value.UserId,
        result.Value.Email,
        result.Value.FirstName,
        result.Value.LastName));
    }

    return TypedResults.NotFound();
  }
}

internal sealed record UserResponse(Guid UserId, string Email, string FirstName, string LastName);