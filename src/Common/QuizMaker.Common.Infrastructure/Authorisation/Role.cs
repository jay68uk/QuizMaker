namespace QuizMaker.Common.Infrastructure.Authorisation;

public sealed class Role
{
  public static readonly Role Administrator = new("Administrator");
  public static readonly Role Member = new("Member");

  private Role(string name)
  {
    Name = name;
  }

  private Role()
  {
  }

  public string Name { get; private set; }
}