using System.Security.Claims;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using QuizMaker.Common.Application.Messaging;
using QuizUser.Infrastructure.Authentication;

namespace QuizUser.Features;

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
            return TypedResults.Ok(result.Value);
        }

        return TypedResults.NotFound();
    }
}

internal sealed record RequestUserByIdQuery(Guid userId) : IQuery<UserResponse>;

internal sealed record UserResponse(string FirstName, string LastName);
