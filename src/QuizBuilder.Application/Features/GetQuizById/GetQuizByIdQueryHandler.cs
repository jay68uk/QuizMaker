using Ardalis.Result;
using QuizMaker.Domain.Quiz;
using QuizMaker.SharedKernel.Messaging;

namespace QuizMaker.Application.Features.GetQuizById;

internal sealed class GetQuizByIdQueryHandler : IQueryHandler<RequestQuizByIdQuery, QuizResponse>
{
  public GetQuizByIdQueryHandler(IQuizRepository quizRepository)
  {
      
  }
  
  public Task<Result<QuizResponse>> Handle(RequestQuizByIdQuery request, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }
}