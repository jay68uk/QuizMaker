using FluentAssertions;
using QuizBuilder.Domain.Question;
using QuizBuilder.Domain.UnitTests.TestData;

namespace QuizBuilder.Domain.UnitTests;

public class QuizQuestionsTests
{
  private const string QuizName = "The Quiz";
  private static readonly Guid UserId = Guid.NewGuid();
  private static readonly DateTimeOffset Created = DateTimeOffset.Now; 

  [Fact]
  public void Should_AddQuestion_ToEmptyQuizQuestions()
  {
    var quiz = Quiz.Quiz.Create(QuizName, Created, UserId);
    
    quiz.Add(QuestionsDirectory.Question1());

    quiz.NumberOfQuestions.Should().Be(1);
  }
  
  [Fact]
  public void Should_AddQuestion_ToPopulatedQuizQuestions()
  {
    var quiz = Quiz.Quiz.Create(QuizName, Created, UserId);
    
    quiz.Add(QuestionsDirectory.Question1());
    quiz.Add(QuestionsDirectory.Question2());

    quiz.NumberOfQuestions.Should().Be(2);
  }
  
  [Fact]
  public void Should_RenumberQuestions_InCorrectOrder_WhenAddInvoked()
  {
    var quiz = Quiz.Quiz.Create(QuizName, Created, UserId);
    
    quiz.Add(QuestionsDirectory.Question1());
    quiz.Add(QuestionsDirectory.Question3());
    quiz.Add(QuestionsDirectory.Question2());

    quiz.RenumberQuestions();
    var question = quiz.GetQuestions().ToList();

    question.Should().HaveCount(3);
    question[0].Number.Value.Should().Be(QuestionsDirectory.QuestionNumber1);
    question[1].Number.Value.Should().Be(QuestionsDirectory.QuestionNumber2);
    question[2].Number.Value.Should().Be(QuestionsDirectory.QuestionNumber3);
  }
  
  [Fact]
  public void Should_DeleteQuestion_And_RenumberQuestions_WhenDeleteInvoked()
  {
    var quiz = Quiz.Quiz.Create(QuizName, Created, UserId);
    
    quiz.Add(QuestionsDirectory.Question1());
    quiz.Add(QuestionsDirectory.Question3());
    quiz.Add(QuestionsDirectory.Question2());

    quiz.Delete(QuestionsDirectory.Question2());
    quiz.RenumberQuestions();
    var question = quiz.GetQuestions().ToList();

    question.Should().HaveCount(2);
    question[0].Number.Value.Should().Be(QuestionsDirectory.QuestionNumber1);
    question[1].Number.Value.Should().Be(QuestionsDirectory.QuestionNumber2);
  }

  [Fact]
  public void Should_CascadeSoftDelete_WhenQuizSoftDeleteIsInvoked()
  {
      var quiz = Quiz.Quiz.Create(QuizName, Created, UserId);
      quiz.Add(QuestionsDirectory.Question1());
      quiz.Add(QuestionsDirectory.Question3());

      quiz.SoftDelete(QuestionsDirectory.DateDeleted);
      var questions = quiz.GetQuestions().ToList();

      quiz.IsDeleted.Should().BeTrue();
      quiz.DeletedDate.Should().NotBeNull();
      questions.Should().HaveCount(2);
      questions[0].IsDeleted.Should().BeTrue();
      questions[0].DeletedDate.Should().Be(QuestionsDirectory.DateDeleted);
      questions[1].IsDeleted.Should().BeTrue();
      questions[1].DeletedDate.Should().Be(QuestionsDirectory.DateDeleted);
  }

  [Fact]
  public void Should_CascadeUndoSoftDelete_WhenQuizUndoDeleteIsInvoked()
  {
      var quiz = Quiz.Quiz.Create(QuizName, Created, UserId);
      quiz.Add(QuestionsDirectory.Question1());
      quiz.Add(QuestionsDirectory.Question3());
      quiz.SoftDelete(QuestionsDirectory.DateDeleted);
    
      quiz.UndoDelete();
      var questions = quiz.GetQuestions().ToList();
      
      quiz.IsDeleted.Should().BeFalse();
      quiz.DeletedDate.Should().BeNull();
      questions[0].IsDeleted.Should().BeFalse();
      questions[0].DeletedDate.Should().Be(null);
      questions[1].IsDeleted.Should().BeFalse();
      questions[0].DeletedDate.Should().Be(null);
  }
}
