using FluentAssertions;
using QuizMaker.Domain.Question;

namespace QuizBuilder.UnitTests.Domain;

public class QuestionsTests
{
  [Fact]
  public void Should_AddQuestion_WhenQuestionIsValid()
  {
    var sut = Questions.Initialise();
    var newQuestion = Question.Create(new Description("A question"), new QuestionNumber(sut.Count() + 1));

    sut.Add(newQuestion);

    sut.Count().Should().Be(1);
  }
  
  [Fact]
  public void Should_DeleteQuestionFromList_WhenNumberMatches()
  {
    var sut = Questions.Initialise();
    var newQuestion = Question.Create(new Description("A question"), new QuestionNumber(sut.Count() + 1));

    sut.Add(newQuestion);
    
    sut.Delete(newQuestion);
    
    sut.Count().Should().Be(0);
  }
  
  [Fact]
  public void Should_RenumberQuestions_WhenAQuestionIsDeleted()
  {
    var sut = Questions.Initialise();
    Question? deleteQuestion = null;

    foreach (var question in QuestionEnum(0))
    {
      sut.Add(question);
      if (question.Number.Value == 2)
      {
        deleteQuestion = question;
      }
    }
    
    sut.Delete(deleteQuestion!);
    sut.RenumberQuestions();
    
    sut.Count().Should().Be(2);
    var result = sut.GetQuestions().Max(q => q.Number.Value);

    result.Should().Be(2);
  }

  private static IEnumerable<Question> QuestionEnum(int seed)
  {
    yield return Question.Create(new Description("A question 1"), new QuestionNumber(seed + 1));
    yield return Question.Create(new Description("A question 2"), new QuestionNumber(seed + 2));
    yield return Question.Create(new Description("A question 3"), new QuestionNumber(seed + 3));
  }
}