using QuizUser.Features.Users;

namespace QuizUser.Abstractions.Users;

public interface IUserRepository
{
  Task<User?> GetAsync(Guid id, CancellationToken cancellationToken = default);

  void Insert(User user);
}