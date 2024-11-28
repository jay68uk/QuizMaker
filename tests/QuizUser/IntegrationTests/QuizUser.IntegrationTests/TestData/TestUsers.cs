using Bogus;

namespace QuizUser.IntegrationTests.TestData;

public static class TestUsers
{
  private static readonly Faker FakerInstance = new();

  public static Dictionary<string, string> UserValid => new()
  {
    { "Email", FakerInstance.Internet.Email() },
    { "Password", FakerInstance.Internet.Password() },
    { "FirstName", FakerInstance.Name.FirstName() },
    { "LastName", FakerInstance.Name.LastName() }
  };

  public static Dictionary<string, string> UserInValidEmail => new()
  {
    { "Email", FakerInstance.Lorem.Word() },
    { "Password", FakerInstance.Internet.Password() },
    { "FirstName", FakerInstance.Name.FirstName() },
    { "LastName", FakerInstance.Name.LastName() }
  };
}