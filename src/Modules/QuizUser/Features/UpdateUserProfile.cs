using System.Net;
using System.Security.Claims;
using FastEndpoints;
using MediatR;
using QuizUser.Infrastructure.Authentication;

namespace QuizUser.Features;

internal sealed class UpdateUserProfile(ISender sender) : Endpoint<UpdateUserRequest>
{
    public override void Configure()
    {
        Put("/users/profile");
    }
    
    public override async Task HandleAsync(UpdateUserRequest request, CancellationToken ct)
    {
        var userId = User.GetUserId();
        var result = await sender.Send(new UpdateUserCommand(
            userId,
            request.FirstName,
            request.LastName), ct);

        if (result.IsSuccess)
        {
            await SendAsync(null, (int)HttpStatusCode.Accepted, ct);
        }
        else
        {
            await SendErrorsAsync((int)HttpStatusCode.BadRequest, ct);     
        }
       
    }
}

internal sealed record UpdateUserCommand(Guid UserId, string RequestFirstName, string RequestLastName) 
    : QuizMaker.Common.Application.Messaging.ICommand;

internal sealed record UpdateUserRequest(string FirstName, string LastName);
