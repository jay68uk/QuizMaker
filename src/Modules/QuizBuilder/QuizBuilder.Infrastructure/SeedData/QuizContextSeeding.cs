using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuizBuilder.Domain.Question;
using QuizBuilder.Domain.Quiz;
using QuizBuilder.Infrastructure.Data;

namespace QuizBuilder.Infrastructure.SeedData;

public static class QuizContextSeeding
{
    public static void InitialiseQuizSeeding(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<QuizDbContext>();
        Seed(context).Wait();
    }
    
    private static async Task Seed(QuizDbContext context)
    {
        await context.Database.MigrateAsync();

        if (context.Quizzes.Any())
        {
            return;
        }

        var quizzes = new List<Quiz>
        {
            Quiz.Create("General Knowledge Quiz", DateTimeOffset.UtcNow, Guid.NewGuid()),
            Quiz.Create("Science Quiz", DateTimeOffset.UtcNow, Guid.NewGuid()),
            Quiz.Create("History Quiz", DateTimeOffset.UtcNow, Guid.NewGuid()),
            Quiz.Create("Movie Quiz", DateTimeOffset.UtcNow, Guid.NewGuid())
        };
        
        quizzes[0].Add(Question.Create(new Description("What is the capital of France?"), new QuestionNumber(1), quizzes[0].Id));
        quizzes[0].Add(Question.Create(new Description("Who wrote 'To Kill a Mockingbird'?"), new QuestionNumber(2), quizzes[0].Id));

        quizzes[1].Add(Question.Create(new Description("What planet is known as the Red Planet?"), new QuestionNumber(1), quizzes[1].Id));
        quizzes[1].Add(Question.Create(new Description("What is the symbol for water?"), new QuestionNumber(2), quizzes[1].Id));

        quizzes[2].Add(Question.Create(new Description("Who was the first President of the United States?"), new QuestionNumber(1), quizzes[2].Id));
        quizzes[2].Add(Question.Create(new Description("When was the Declaration of Independence signed?"), new QuestionNumber(2), quizzes[2].Id));
        
        context.Quizzes.AddRange(quizzes);
        
        await context.SaveChangesAsync();
    }
}
