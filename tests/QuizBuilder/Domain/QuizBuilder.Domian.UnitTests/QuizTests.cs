using Ardalis.Result;
using FluentAssertions;
using QuizMaker.Domain.Quiz;

namespace QuizBuilder.UnitTests.Domain;

public class QuizTests
{
  private const string QuizName = "The Quiz";
  private static readonly Guid UserId = Guid.NewGuid();
  private static readonly DateTimeOffset Created = DateTimeOffset.Now; 
  private static readonly DateTimeOffset RanDate = DateTimeOffset.Now; 
  
  [Fact]
  public void Should_CreateQuiz_WhenInputValid()
  {
    var sut = Quiz.Create(QuizName, Created, UserId);

    sut.NumberOfQuestions.Should().Be(0);
    sut.Status.Should().Be(QuizStatus.Draft);
    sut.CreatedBy.Should().Be(UserId);
    sut.CreatedDate.Should().Be(Created);
    sut.Name.Value.Should().Be(QuizName);
  }
  
  [Theory]
  [InlineData([""])]
  [InlineData([null])]
  public void Should_ThrowException_WhenNameIsNullOrEmpty(string? name)
  {
    var user = Guid.NewGuid();
    var created = DateTimeOffset.Now;
    var sut = () => Quiz.Create(name, created, user);

    sut.Should().Throw<ArgumentException>();
  }

  [Fact]
  public void ChangeStatus_Should_ChangeStatus_WhenInvoked()
  {
    var sut = Quiz.Create(QuizName, Created, UserId);
    
    sut.ChangeStatus(QuizStatus.Ready);

    sut.Status.Should().Be(QuizStatus.Ready);
  }

  [Fact]
  public void Finish_Should_SetCorrectStatus_WhenInvoked()
  {
    var sut = Quiz.Create(QuizName, Created, UserId);
    sut.ChangeStatus(QuizStatus.Running);
    
    var result = sut.FinishQuiz();

    result.IsSuccess.Should().BeTrue();
    sut.Status.Should().Be(QuizStatus.Completed);
  }
  
  [Fact]
  public void Finish_Should_ReturnResultError_WhenStatusIsNot_Running()
  {
    var sut = Quiz.Create(QuizName, Created, UserId);
    
    var result = sut.FinishQuiz();

    result.IsError().Should().BeTrue();
    result.Errors.Should().Contain(QuizErrors.CompletedError);
    sut.Status.Should().Be(QuizStatus.Draft);
  }
  
  [Fact]
  public void Start_Should_SetCorrectStatusAndDate_WhenInvoked()
  {
    var sut = Quiz.Create(QuizName, Created, UserId);
    sut.ChangeStatus(QuizStatus.Ready);
    
    var result = sut.StartQuiz(RanDate);

    result.IsSuccess.Should().BeTrue();
    sut.Status.Should().Be(QuizStatus.Running);
    sut.RanDate.Should().Be(RanDate);
  }
  
  [Fact]
  public void Start_Should_ReturnResultError_WhenStatusIsNot_Ready()
  {
    var sut = Quiz.Create(QuizName, Created, UserId);
    
    var result = sut.StartQuiz(RanDate);

    result.IsError().Should().BeTrue();
    result.Errors.Should().Contain(QuizErrors.StartError);
    sut.Status.Should().Be(QuizStatus.Draft);
    sut.RanDate.Should().BeNull();
  }
  
  [Theory]
  [InlineData(QuizStatus.Draft)]
  [InlineData(QuizStatus.Completed)]
  public async Task Prepare_Should_SetAccessCode_WhenInvoked(QuizStatus status)
  {
    var sut = Quiz.Create(QuizName, Created, UserId);
    sut.ChangeStatus(status);
    
    var result = await sut.PrepareQuizAsync();

    result.IsSuccess.Should().BeTrue();
    sut.Status.Should().Be(QuizStatus.Ready);
    sut.AccessCode.AccessCode.Should().NotBeNullOrWhiteSpace();
    sut.AccessCode.QrCode.Should().NotBeNullOrWhiteSpace();
    sut.AccessCode.QuizLink.Should().NotBeNullOrWhiteSpace();
  }
  
  [Theory]
  [InlineData(QuizStatus.Running, QuizErrors.PrepareError)]
  [InlineData(QuizStatus.Ready, QuizErrors.PrepareAlreadyDoneError)]
  [InlineData(QuizStatus.Deleted, QuizErrors.PrepareError)]
  public async Task Prepare_Should_ReturnResultError_WhenCurrentStatusIsIncorrect(QuizStatus status, string expectedError)
  {
    var sut = Quiz.Create(QuizName, Created, UserId);
    sut.ChangeStatus(status);
    
    var result = await sut.PrepareQuizAsync();

    result.IsError().Should().BeTrue();
    result.Errors.Should().Contain(expectedError);
    sut.Status.Should().Be(status);
  }
}


