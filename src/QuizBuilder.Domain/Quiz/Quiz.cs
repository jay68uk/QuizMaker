using Ardalis.Result;
using QuizMaker.Domain.Question;
using QuizMaker.Domain.QuizAccessCode;
using QuizMaker.SharedKernel;

namespace QuizMaker.Domain.Quiz;

public sealed class Quiz : Entity
{
   public QuizName Name { get; private set; }
   public QuizAccessCode.QuizAccessCode AccessCode { get; private set;}
   public DateTimeOffset CreatedDate { get; private set; }
   public DateTimeOffset? RanDate { get; private set; }
   public Guid CreatedBy { get; private set; }
   public Questions Questions { get; private set; }
   public QuizStatus Status { get; private set; } 

   private Quiz(Guid id, QuizName name, DateTimeOffset createdDate, Guid createdBy) : base(id)
   {
      Name = name;
      CreatedDate = createdDate;
      CreatedBy = createdBy;
      Status = QuizStatus.Draft;
      Questions = Questions.Initialise();
   }

   public static Quiz Create(string? quizName, DateTimeOffset created, Guid createdBy)
   {
      var quiz = new Quiz(Ulid.NewUlid().ToGuid(), new QuizName(quizName), created, createdBy);
      return quiz;
   }
   
   public int NumberOfQuestions => Questions.Count();

   public void ChangeStatus(QuizStatus status)
   {
      Status = status;
   }

   public Result StartQuiz(DateTimeOffset ranDate)
   {
      if (Status is not QuizStatus.Ready)
      {
         return Result.Error(QuizErrors.StartError);
      }
      
      RanDate = ranDate;
      Status = QuizStatus.Running;
      return Result.Success();
   }

   public Result FinishQuiz()
   {
      if (Status is not QuizStatus.Running)
      {
         return Result.Error(QuizErrors.CompletedError);
      }
      
      Status = QuizStatus.Completed;
      return Result.Success();
   }

   public async Task<Result> PrepareQuizAsync()
   {
      switch (Status)
      {
         case QuizStatus.Ready:
            return Result.Error(QuizErrors.PrepareAlreadyDoneError);
         case QuizStatus.Draft:
         case QuizStatus.Completed:
            AccessCode = await QuizAccessCode.QuizAccessCode.Create();
            Status = QuizStatus.Ready;
            return Result.Success();
         case QuizStatus.Running:
         case QuizStatus.Deleted:
         default:
            return Result.Error(QuizErrors.PrepareError);
      }
   }
}