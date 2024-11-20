using QuizUser.Features.Users;

namespace QuizUser.UnitTests.TestData;

public static class UserTestData
{
  public static User DefaultUser()
  {
    return User.Create("me@me.com", "Joe", "Bloggs", Guid.NewGuid().ToString());
  }
}