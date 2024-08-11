using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace QuizBuilder.Presentation.Features.QuizEndpoints;

public class GetById : EndpointWithoutRequest<Results<Ok<GetByIdResponse>, 
                                                      NotFound, 
                                                      ProblemDetails>>
{
  public override void Configure() => Get("/quiz/{QuizId}");

  public override async Task<Results<Ok<GetByIdResponse>, 
    NotFound, 
    ProblemDetails>> HandleAsync(CancellationToken ct)
  {
    var quizId = Route<Guid>("QuizId");

    return TypedResults.NotFound();
  }
}

public class GetByIdResponse
{
}