namespace QuizBuilder.Application.UnitTests.TestData;

public class QuestionResponseBuilder
{
  private Guid _id;
  private string? _description;
  private int _number;
  private QuestionResponseBuilder(Guid id)
  {
    _id = id;
  }

  public static QuestionResponseBuilder Default() => new QuestionResponseBuilder(Ulid.NewUlid().ToGuid());
  
  public static QuestionResponseBuilder DefaultWithId(Guid id) => new QuestionResponseBuilder(id);
  
  public QuestionResponseBuilder WithDescription(string description)
  {
    _description = description;
    return this;
  }
  
  public QuestionResponseBuilder WithNumber(int number)
  {
    _number = number;
    return this;
  }

  public QuestionResponseBuilder Build() => this;
}