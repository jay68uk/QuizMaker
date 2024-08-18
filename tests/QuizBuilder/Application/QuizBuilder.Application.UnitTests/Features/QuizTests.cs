using QuizBuilder.Application.Features.GetQuizById;
using QuizBuilder.Application.UnitTests.TestDoubles;
using QuizBuilder.Domain.Quiz;

namespace QuizBuilder.Application.UnitTests.Features;

public class QuizTests
{
  private readonly IQuizRepository _quizRepository = new QuizRepositoryMock();
  public QuizTests()
  {
      
  }
  
  [Fact]
  public async Task Handler_ShouldReturnQuiz_WhenQuizIdIsFound()
  {
    var id = Ulid.NewUlid().ToGuid();
    var query = new RequestQuizByIdQuery(id);

    var sut = new GetQuizByIdQueryHandler(_quizRepository);

    var result = await sut.Handle(query, default);
  }
}