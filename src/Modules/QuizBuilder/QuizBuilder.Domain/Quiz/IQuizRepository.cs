namespace QuizBuilder.Domain.Quiz;

public interface IQuizRepository
{
  Task AddAsync(Quiz quiz);

  Task UpdateAsync(Quiz quiz);

  Task DeleteByIdAsync(Guid quizId);

}