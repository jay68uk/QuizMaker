using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using QuizBuilder.Application.Features.GetQuizById;

namespace QuizBuilder.Presentation.Features.QuizEndpoints;

public class GetById(ISender sender) : EndpointWithoutRequest
<Results<
    Ok<QuizResponse>,
    NotFound,
    ProblemDetails>>
{
    public override void Configure()
    {
        Get("/quizzes/{QuizId}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<QuizResponse>,
        NotFound,
        ProblemDetails>> ExecuteAsync(CancellationToken ct)
    {
        var x = Route<string>("QuizId");
        Console.Write(x);
        var quizId = Route<Guid>("QuizId");

        var query = new RequestQuizByIdQuery(quizId);

        var result = await sender.Send(query, ct);

        if (result.IsSuccess)
        {
            return TypedResults.Ok(result.Value);
        }

        return TypedResults.NotFound();
    }
}
