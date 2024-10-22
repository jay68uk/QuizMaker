using System.Diagnostics.CodeAnalysis;
using QuizBuilder.Application.Features.GetQuizById;
using QuizBuilder.Domain.Quiz;

namespace QuizBuilder.Presentation.UnitTests.TestData;

public class QuizResponseBuilder
{
  private readonly Guid _id;
  private string? _name;
  private string? _accessCode;
  private string? _qrCode;
  private DateTimeOffset _createdDate;
  private DateTimeOffset? _ranDate;
  private Guid _createdBy;
  private QuizStatus _status = QuizStatus.Draft;
  
  private QuizResponseBuilder(Guid id)
  {
    _id = id;
  }

  public static QuizResponseBuilder Default() => new(Ulid.NewUlid().ToGuid());
  
  public static QuizResponseBuilder DefaultWithId(Guid id) => new(id);
  
  public QuizResponseBuilder WithName(string name)
  {
    _name = name;
    return this;
  }
  
  public QuizResponseBuilder HasAccessCode(bool hasAccessCode = false, Guid? accessCode = null)
  {
    switch (hasAccessCode)
    {
      case false when accessCode is null:
        return this;
      case true when accessCode is null:
        _accessCode = Ulid.NewUlid().ToGuid().ToString();
        _qrCode = string.Empty;
        break;
      default:
        _accessCode = accessCode.ToString();
        _qrCode = string.Empty; 
        break;
    }

    return this;
  }

  public QuizResponseBuilder CreatedDate(DateTimeOffset createdDate)
  {
    _createdDate = createdDate;
    return this;
  }

  public QuizResponseBuilder CreatedBy(Guid userId)
  {
    _createdBy = userId;
    return this;
  }

  public QuizResponseBuilder RanDate(DateTimeOffset ranDate)
  {
    _ranDate = ranDate;
    return this;
  }

  public QuizResponseBuilder WithStatus(QuizStatus status)
  {
    _status = status;
    return this;
  }

  public static implicit operator QuizResponse(QuizResponseBuilder builder) => builder.Build();

  [SuppressMessage("Major Code Smell", "S3928:Parameter names used into ArgumentException constructors should match an existing one ")]
  private QuizResponse Build()
  {
    return new QuizResponse(){Id = _id,
      Name = _name ?? throw new ArgumentNullException(nameof(_name)),
      AccessCode = _accessCode!,
      CreatedDate = _createdDate,
      RanDate = _ranDate,
      CreatedBy = _createdBy,
      Status = _status,
      Questions = []};
  }
}
