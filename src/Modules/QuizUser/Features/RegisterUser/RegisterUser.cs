using System.Net;
using FastEndpoints;
using MediatR;

namespace QuizUser.Features.RegisterUser;

internal sealed class RegisterUser(ISender sender) : Endpoint<RegisterUserRequest>
{
  public override void Configure()
  {
    Post("/users/register");
    AllowAnonymous();
  }

  public override async Task HandleAsync(RegisterUserRequest request, CancellationToken ct)
  {
    var result = await sender.Send(new RegisterUserCommand(
      request.Email,
      request.Password,
      request.FirstName,
      request.LastName), ct);

    if (result.IsSuccess)
    {
      await SendCreatedAtAsync("UserProfile", result.Value, result.Value, true, ct);
    }
    else
    {
      await SendErrorsAsync((int)HttpStatusCode.BadRequest, ct);
    }
  }
}

internal sealed record RegisterUserRequest(string Email, string Password, string FirstName, string LastName);