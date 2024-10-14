using Ardalis.Result;
using FluentAssertions;
using QuizBuilder.Domain.Quiz;
using QuizBuilder.Domain.UnitTests.TestData;

namespace QuizBuilder.Domain.UnitTests;

public class QuizTests
{
  private const string QuizName = "The Quiz";
  private static readonly Guid UserId = Guid.NewGuid();
  private static readonly DateTimeOffset Created = DateTimeOffset.Now; 
  private static readonly DateTimeOffset RanDate = DateTimeOffset.Now; 
  
  [Fact]
  public void Should_CreateQuiz_WhenInputValid()
  {
    var quiz = Quiz.Quiz.Create(QuizName, Created, UserId);

    quiz.NumberOfQuestions.Should().Be(0);
    quiz.Status.Should().Be(QuizStatus.Draft);
    quiz.CreatedBy.Should().Be(UserId);
    quiz.CreatedDate.Should().Be(Created);
    quiz.Name.Value.Should().Be(QuizName);
  }
  
  [Theory]
  [InlineData([""])]
  [InlineData([null])]
  public void Should_ThrowException_WhenNameIsNullOrEmpty(string? name)
  {
    var user = Guid.NewGuid();
    var created = DateTimeOffset.Now;
    var quizFunc = () => Quiz.Quiz.Create(name, created, user);

    quizFunc.Should().Throw<ArgumentException>();
  }

  [Fact]
  public void ChangeStatus_Should_ChangeStatus_WhenInvoked()
  {
    var quiz = Quiz.Quiz.Create(QuizName, Created, UserId);
    
    quiz.ChangeStatus(QuizStatus.Ready);

    quiz.Status.Should().Be(QuizStatus.Ready);
  }

  [Fact]
  public void Finish_Should_SetCorrectStatus_WhenInvoked()
  {
    var quiz = Quiz.Quiz.Create(QuizName, Created, UserId);
    quiz.ChangeStatus(QuizStatus.Running);
    
    var result = quiz.FinishQuiz();

    result.IsSuccess.Should().BeTrue();
    quiz.Status.Should().Be(QuizStatus.Completed);
  }
  
  [Fact]
  public void Finish_Should_ReturnResultError_WhenStatusIsNot_Running()
  {
    var quiz = Quiz.Quiz.Create(QuizName, Created, UserId);
    
    var result = quiz.FinishQuiz();

    result.IsError().Should().BeTrue();
    result.Errors.Should().Contain(QuizErrors.CompletedError);
    quiz.Status.Should().Be(QuizStatus.Draft);
  }
  
  [Fact]
  public void Start_Should_SetCorrectStatusAndDate_WhenInvoked()
  {
    var quiz = Quiz.Quiz.Create(QuizName, Created, UserId);
    quiz.ChangeStatus(QuizStatus.Ready);
    
    var result = quiz.StartQuiz(RanDate);

    result.IsSuccess.Should().BeTrue();
    quiz.Status.Should().Be(QuizStatus.Running);
    quiz.RanDate.Should().Be(RanDate);
  }
  
  [Fact]
  public void Start_Should_ReturnResultError_WhenStatusIsNot_Ready()
  {
    var quiz = Quiz.Quiz.Create(QuizName, Created, UserId);
    
    var result = quiz.StartQuiz(RanDate);

    result.IsError().Should().BeTrue();
    result.Errors.Should().Contain(QuizErrors.StartError);
    quiz.Status.Should().Be(QuizStatus.Draft);
    quiz.RanDate.Should().BeNull();
  }
  
  [Theory]
  [InlineData(QuizStatus.Draft)]
  [InlineData(QuizStatus.Completed)]
  public async Task Prepare_Should_SetAccessCode_WhenInvoked(QuizStatus status)
  {
    var quiz = Quiz.Quiz.Create(QuizName, Created, UserId);
    quiz.ChangeStatus(status);
    
    var result = await quiz.PrepareQuizAsync();

    result.IsSuccess.Should().BeTrue();
    quiz.Status.Should().Be(QuizStatus.Ready);
    quiz.AccessCode.AccessCode.Should().NotBeNullOrWhiteSpace();
    quiz.AccessCode.QrCode.Should().NotBeNullOrWhiteSpace();
  }
  
  [Theory]
  [InlineData(QuizStatus.Running, QuizErrors.PrepareError)]
  [InlineData(QuizStatus.Ready, QuizErrors.PrepareAlreadyDoneError)]
  [InlineData(QuizStatus.Deleted, QuizErrors.PrepareError)]
  public async Task Prepare_Should_ReturnResultError_WhenCurrentStatusIsIncorrect(QuizStatus status, string expectedError)
  {
    var quiz = Quiz.Quiz.Create(QuizName, Created, UserId);
    quiz.ChangeStatus(status);
    
    var result = await quiz.PrepareQuizAsync();

    result.IsError().Should().BeTrue();
    result.Errors.Should().Contain(expectedError);
    quiz.Status.Should().Be(status);
  }

  [Fact]
  public void Should_SoftDelete_WhenSoftDeleteIsInvoked()
  {
      var quiz = Quiz.Quiz.Create(QuizName, Created, UserId);

      quiz.SoftDelete(QuestionsDirectory.DateDeleted);

      quiz.IsDeleted.Should().BeTrue();
      quiz.DeletedDate.Should().Be(QuestionsDirectory.DateDeleted);
  }
  
  [Fact]
  public void Should_UndoSoftDelete_WhenUndoDeleteIsInvoked()
  {
      var quiz = Quiz.Quiz.Create(QuizName, Created, UserId);
      quiz.SoftDelete(QuestionsDirectory.DateDeleted);
    
      quiz.UndoDelete();
      
      quiz.IsDeleted.Should().BeFalse();
      quiz.DeletedDate.Should().BeNull();
  }
}


