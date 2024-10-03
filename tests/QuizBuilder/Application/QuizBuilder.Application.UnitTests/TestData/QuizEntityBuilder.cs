using QuizBuilder.Domain.Question;
using QuizBuilder.Domain.Quiz;
using QuizBuilder.Domain.QuizAccessCode;
using QuizMaker.Common.Domain;

namespace QuizBuilder.Application.UnitTests.TestData;

public class QuizEntityBuilder : Entity
{
  private List<Question> _questions = [];
  private QuizName _name = null!;
  private QuizAccessCode? _accessCode;
  private DateTimeOffset _createdDate;
  private DateTimeOffset? _ranDate = null;
  private Guid _createdBy;
  private QuizStatus _status = QuizStatus.Draft;

  private QuizEntityBuilder(Guid id) : base(id)
  {
    
  }

  public static QuizEntityBuilder Default() => new(Ulid.NewUlid().ToGuid());
  
  public static QuizEntityBuilder DefaultWithId(Guid id) => new(id);
  
  public QuizEntityBuilder WithName(string name)
  {
    _name = new QuizName(name);
    return this;
  }

  public async Task<QuizEntityBuilder> HasAccessCode(bool hasAccessCode = false)
  {
    if (hasAccessCode)
    {
      _accessCode = await QuizAccessCode.Create();
    }
    return this;
  }

  public QuizEntityBuilder CreatedDate(DateTimeOffset createdDate)
  {
    _createdDate = createdDate;
    return this;
  }

  public QuizEntityBuilder CreatedBy(Guid userId)
  {
    _createdBy = userId;
    return this;
  }

  public QuizEntityBuilder RanDate(DateTimeOffset ranDate)
  {
    _ranDate = ranDate;
    return this;
  }

  public QuizEntityBuilder WithStatus(QuizStatus status)
  {
    _status = status;
    return this;
  }

  public QuizEntityBuilder Build() => this;
}
