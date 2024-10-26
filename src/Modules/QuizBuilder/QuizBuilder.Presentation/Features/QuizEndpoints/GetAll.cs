using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using QuizBuilder.Application.Features.GetQuizById;
using QuizMaker.Common.Application.Messaging;

namespace QuizBuilder.Presentation.Features.QuizEndpoints;

public class GetAll(ISender sender) : EndpointWithoutRequest
<Results<
    Ok<QuizResponse>,
    NotFound,
    ProblemDetails>>
{
    public override void Configure()
    {
        Get("/quizzes");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<QuizResponse>,
        NotFound,
        ProblemDetails>> ExecuteAsync(CancellationToken ct)
    {
        var userId = Guid.NewGuid();
        var query = new RequestQuizByUserQuery(userId);

        var result = await sender.Send(query, ct);

        if (result.IsSuccess)
        {
            return TypedResults.Ok(result.Value);
        }

        return TypedResults.NotFound();
    }
}

public class RequestQuizByUserQuery : IQuery<QuizResponse>
{
    public RequestQuizByUserQuery(Guid userId)
    {
        throw new NotImplementedException();
    }
}
