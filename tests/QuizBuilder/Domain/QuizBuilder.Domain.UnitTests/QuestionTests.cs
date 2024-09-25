using FluentAssertions;
using QuizBuilder.Domain.Question;
using QuizBuilder.Domain.UnitTests.TestData;

namespace QuizBuilder.Domain.UnitTests;

public class QuestionTests
{
  [Fact]
  public void Should_CreateQuestion_WhenInputValid()
  {
    var description = new Description(QuestionsDirectory.Description);
    var questionNumber = new QuestionNumber(QuestionsDirectory.QuestionNumber1);
    var question = Question.Question.Create(description, questionNumber);

    question.Description.Should().BeEquivalentTo(description);
    question.Number.Should().BeEquivalentTo(questionNumber);
    question.Id.Should().NotBe(Guid.Empty);
  }
  
  [Fact]
  public void Should_UpdateQuestionNumber_WhenValid()
  {
    var question = Question.Question.Create(new Description(QuestionsDirectory.Description),
      new QuestionNumber(QuestionsDirectory.QuestionNumber1));
    var newNumber = new QuestionNumber(QuestionsDirectory.QuestionNumber2);

    question.UpdateNumbering(newNumber);

    question.Number.Should().BeEquivalentTo(newNumber);
  }

  [Theory]
  [InlineData([0])]
  [InlineData([-1])]
  public void QuestionNumber_Should_ThrowException_WhenZeroOrNegative(int number)
  {
    var result = () => new QuestionNumber(number);

    result.Should().ThrowExactly<ArgumentException>();
  }

  [Fact]
  public void QuestionNumber_Should_HaveValueSet_WhenValid()
  {
    var questionNumber =  new QuestionNumber(QuestionsDirectory.QuestionNumber2);

    questionNumber.Value.Should().Be(QuestionsDirectory.QuestionNumber2);
  }
  
  [Theory]
  [InlineData([""])]
  [InlineData([null])]
  public void Description_Should_ThrowException_WhenNullOrEmpty(string? description)
  {
    var descriptionFunc = () => new Description(description);

    descriptionFunc.Should().Throw<ArgumentException>();
  }
  
  [Fact]
  public void Description_Should_HaveValueSet_WhenValid()
  {
    var description =  new Description(QuestionsDirectory.Description);
    var descriptionValue = description.Value;
    
    descriptionValue.Should().Be(QuestionsDirectory.Description);
  }

  [Fact]
  public void Equals_Should_ReturnTrue_WhenQuestionsAreTheSame()
  {
    var question = QuestionsDirectory.Question1();
    var equivalentQuestion = QuestionsDirectory.Question1();

    var questionsEqual = question.Equals(equivalentQuestion);
    
    questionsEqual.Should().BeTrue();
  }
  
  [Fact]
  public void Equals_Should_ReturnFalse_WhenQuestionsAreDifferent()
  {
    var question = QuestionsDirectory.Question1();
    var nonEquivalentQuestion = QuestionsDirectory.Question2();

    var questionsEqual = question.Equals(nonEquivalentQuestion);
    
    questionsEqual.Should().BeFalse();
  }
}