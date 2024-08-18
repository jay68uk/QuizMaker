using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using QuizBuilder.Application.Features.GetQuizById;

namespace QuizBuilder.Presentation.Features.QuizEndpoints;

public class GetById : EndpointWithoutRequest<Results<Ok<QuizResponse>, 
                                                      NotFound, 
                                                      ProblemDetails>>
{
  private readonly ISender _sender;

  public GetById(ISender sender)
  {
    _sender = sender;
  }
  
  public override void Configure() => Get("/quiz/{QuizId}");

  public override async Task<Results<Ok<QuizResponse>, 
    NotFound, 
    ProblemDetails>> ExecuteAsync(CancellationToken ct)
  {
    var quizId = Route<Guid>("QuizId");

    var query = new RequestQuizByIdQuery(quizId);

    var result = await _sender.Send(query, ct);

    if (result.IsSuccess)
    {
      return TypedResults.Ok(result.Value);
    }

    return TypedResults.NotFound();
  }
}
