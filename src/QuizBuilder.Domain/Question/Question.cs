using QuizMaker.SharedKernel;

namespace QuizBuilder.Domain.Question;

public sealed class Question : Entity
{
  public Description Description { get; private set; }
  public QuestionNumber Number { get; private set; }

  private Question(Guid id,Description description, QuestionNumber number) : base(id)
  {
    Description = description;
    Number = number;
  }

  public static Question Create(Description description, QuestionNumber number)
  {
    var question = new Question(Ulid.NewUlid().ToGuid(), description, number);
    
    return question;
  }

  public void UpdateNumbering(QuestionNumber newNumber)
  {
    Number = newNumber;
  }
}