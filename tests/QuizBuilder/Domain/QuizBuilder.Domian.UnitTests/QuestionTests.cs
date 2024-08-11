using System.Runtime.InteropServices.JavaScript;
using FluentAssertions;
using QuizMaker.Domain.Question;

namespace QuizBuilder.UnitTests.Domain;

public class QuestionTests
{
  private const int Question1 = 1;
  private const int Question2 = 2;
  private const string Description = "Question description";
  
  [Fact]
  public void Should_CreateQuestion_WhenInputValid()
  {
    var description = new Description(Description);
    var questionNumber = new QuestionNumber(Question1);
    var sut = Question.Create(description, questionNumber);

    sut.Description.Should().BeEquivalentTo(description);
    sut.Number.Should().BeEquivalentTo(questionNumber);
    sut.Id.Should().NotBe(Guid.Empty);
  }
  
  [Fact]
  public void Should_UpdateQuestionNumber_WhenValid()
  {
    var sut = Question.Create(new Description("A question"), new QuestionNumber(Question1));
    var newNumber = new QuestionNumber(Question2);

    sut.UpdateNumbering(newNumber);

    sut.Number.Should().BeEquivalentTo(newNumber);
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
    var result =  new QuestionNumber(Question2);

    result.Value.Should().Be(Question2);
  }
  
  [Theory]
  [InlineData([""])]
  [InlineData([null])]
  public void Description_Should_ThrowException_WhenNullOrEmpty(string? description)
  {
    var result = () => new Description(description);

    result.Should().Throw<ArgumentException>();
  }
  
  [Fact]
  public void Description_Should_HaveValueSet_WhenValid()
  {
    var sut =  new Description(Description);
    var result = sut.Value;
    
    result.Should().Be(Description);
  }
}