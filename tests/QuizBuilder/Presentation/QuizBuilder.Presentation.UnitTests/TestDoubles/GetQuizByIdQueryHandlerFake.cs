using Ardalis.Result;
using QuizBuilder.Application.Features.GetQuizById;
using QuizBuilder.Presentation.UnitTests.TestData;
using QuizMaker.Common.Application.Messaging;

namespace QuizBuilder.Presentation.UnitTests.TestDoubles;

public class GetQuizByIdQueryHandlerFake : IQueryHandler<RequestQuizByIdQuery, QuizResponse>
{
  public Task<Result<QuizResponse>> Handle(RequestQuizByIdQuery request, CancellationToken cancellationToken)
  {
    return Task.FromResult(Result<QuizResponse>
      .Success(QuizResponseDirectory.QuizResponseDefaultValid(request.Id)));
  }
}