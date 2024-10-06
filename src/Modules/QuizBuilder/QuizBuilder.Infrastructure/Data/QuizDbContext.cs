using Microsoft.EntityFrameworkCore;
using QuizBuilder.Domain.Quiz;

namespace QuizBuilder.Infrastructure.Data;

public class QuizDbContext(DbContextOptions<QuizDbContext> options) : DbContext(options)
{
    public DbSet<Quiz> Quizzes => Set<Quiz>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(QuizDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}
