using QuizBuilder.Domain.Quiz;

namespace QuizBuilder.Application.UnitTests.TestDoubles;

internal class QuizRepositoryMock : IQuizRepository
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