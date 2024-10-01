using QuizBuilder.Application.Features.GetQuizById;
using QuizBuilder.Domain.Quiz;

namespace QuizBuilder.Presentation.UnitTests.TestData;

public class QuizResponseBuilder
{
  private Guid _id;
  private IReadOnlyList<QuestionResponse> _questions = [];
  private string? _name = null!;
  private string? _accessCode;
  private string? _qrCode;
  private DateTimeOffset _createdDate;
  private DateTimeOffset? _ranDate = null;
  private Guid _createdBy;
  private QuizStatus _status = QuizStatus.Draft;
  
  private QuizResponseBuilder(Guid id)
  {
    _id = id;
  }

  public static QuizResponseBuilder Default() => new QuizResponseBuilder(Ulid.NewUlid().ToGuid());
  
  public static QuizResponseBuilder DefaultWithId(Guid id) => new QuizResponseBuilder(id);
  
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
        _qrCode = string.Empty; // TODO how to generate QRCode
        break;
      default:
        _accessCode = accessCode.ToString();
        _qrCode = string.Empty; // TODO how to generate QRCode from access code
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

  private QuizResponse Build()
  {
    return new QuizResponse(_id,
      [],
      _name ?? throw new ArgumentNullException(nameof(_name)),
      _accessCode!,
      _qrCode!,
      _createdDate,
      _ranDate,
      _createdBy,
      _status);
  }
}