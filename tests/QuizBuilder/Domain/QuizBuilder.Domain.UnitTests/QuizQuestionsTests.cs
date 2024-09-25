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
}