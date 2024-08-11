using QuizMaker.Application.Features.GetQuizById;
using QuizMaker.Domain.Quiz;

namespace QuizBuilder.UnitTests.Application;

public class QuizTests
{
  private readonly IQuizRepository _quizRepository = new QuizRepositoryStub();
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

internal class QuizRepositoryStub : IQuizRepository
{
  public Task<Quiz> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<IEnumerable<Quiz>> GetByUserIdAsync(Guid id, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task AddAsync(Quiz quiz)
  {
    throw new NotImplementedException();
  }

  public Task UpdateAsync(Quiz quiz)
  {
    throw new NotImplementedException();
  }

  public Task DeleteByIdAsync(Guid quizId)
  {
    throw new NotImplementedException();
  }
}