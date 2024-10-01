using Microsoft.Extensions.Time.Testing;
using QuizBuilder.Application.Features.GetQuizById;

namespace QuizBuilder.Presentation.UnitTests.TestData;

public static class QuizResponseDirectory
{
  public static QuizResponse  QuizResponseDefaultValid(Guid id)
  {
    var createdDate = new FakeTimeProvider(new DateTimeOffset(2024, 11, 10, 16, 32, 21, TimeSpan.Zero));
    
    return QuizResponseBuilder
      .DefaultWithId(id)
      .WithName("Quiz A")
      .HasAccessCode(false)
      .CreatedDate(createdDate.GetUtcNow());
  }
}