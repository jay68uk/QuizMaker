using QuizBuilder.Domain.Question;

namespace QuizBuilder.Domain.UnitTests.TestData;

public static class QuestionsDirectory
{
  public const int QuestionNumber1 = 1;
  public const int QuestionNumber2= 2;
  public const int QuestionNumber3= 3;
  public const string Description = "Question description";
  
  public static Question.Question Question1()
  {
    var description = new Description(Description);
    var questionNumber = new QuestionNumber(QuestionsDirectory.QuestionNumber1);
    return Question.Question.Create(description, questionNumber);
  }
  
  public static Question.Question Question2()
  {
    var description = new Description(Description);
    var questionNumber = new QuestionNumber(QuestionsDirectory.QuestionNumber2);
    return Question.Question.Create(description, questionNumber);
  }
  
  public static Question.Question Question3()
  {
    var description = new Description(Description);
    var questionNumber = new QuestionNumber(QuestionsDirectory.QuestionNumber3);
    return Question.Question.Create(description, questionNumber);
  }
}