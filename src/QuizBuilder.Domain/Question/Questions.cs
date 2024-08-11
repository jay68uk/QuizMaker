namespace QuizMaker.Domain.Question;

public sealed class Questions
{
  private List<Question> QuestionList { get; set; }

  private Questions()
  {
    QuestionList = new List<Question>();
  }
  
  public static Questions Initialise()
  {
    return new Questions();
  }

  public IEnumerable<Question> GetQuestions() => QuestionList.AsReadOnly();

  public int Count() => QuestionList.Count;

  public void Add(Question question)
  {
    QuestionList.Add(question);
  }

  public void Delete(Question question)
  {
    QuestionList.Remove(question);
  }

  public void RenumberQuestions()
  {
    QuestionList = QuestionList.Select((q, index) =>
    {
      q.UpdateNumbering(new QuestionNumber(index + 1));
      return q;
    }).ToList();
  }
}