namespace QuizMaker.Domain.Quiz;

public interface IQuizRepository
{
  Task<Quiz> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
  
  Task<IEnumerable<Quiz>> GetByUserIdAsync(Guid id, CancellationToken cancellationToken = default);

  Task AddAsync(Quiz quiz);

  Task UpdateAsync(Quiz quiz);

  Task DeleteByIdAsync(Guid quizId);

}