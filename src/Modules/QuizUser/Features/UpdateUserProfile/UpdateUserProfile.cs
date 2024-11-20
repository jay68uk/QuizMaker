using System.Net;
using FastEndpoints;
using MediatR;
using QuizMaker.Common.Infrastructure.Authorisation;
using QuizUser.Features.Authentication;

namespace QuizUser.Features.UpdateUserProfile;

internal sealed class UpdateUserProfile(ISender sender) : Endpoint<UpdateUserRequest>
{
  public override void Configure()
  {
    Put("/users/profile");
    Policies(PolicyNames.MembersProfileUpdate);
  }

  public override async Task HandleAsync(UpdateUserRequest request, CancellationToken ct)
  {
    var userId = User.GetUserId();
    var result = await sender.Send(new UpdateUserCommand(
      userId,
      request.FirstName,
      request.LastName,
      request.Email), ct);

    if (result.IsSuccess)
    {
      await SendAsync(null, (int)HttpStatusCode.Accepted, ct);
      return;
    }

    await SendAsync(result.ValidationErrors, (int)HttpStatusCode.BadRequest, ct);
  }
}

internal sealed record UpdateUserRequest(string FirstName, string LastName, string Email);