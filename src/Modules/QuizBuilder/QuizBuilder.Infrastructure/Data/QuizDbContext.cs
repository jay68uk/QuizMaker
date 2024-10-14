using System.Reflection;
using Microsoft.EntityFrameworkCore;
using QuizBuilder.Domain.Quiz;
using QuizMaker.Common.Domain;

namespace QuizBuilder.Infrastructure.Data;

public class QuizDbContext(DbContextOptions<QuizDbContext> options) : DbContext(options)
{
    public DbSet<Quiz> Quizzes => Set<Quiz>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(QuizDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
        
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
            {
                continue;
            }

            var method = typeof(QuizDbContext)
#pragma warning disable S3011
                .GetMethod(nameof(ApplyGlobalSoftDeleteFilter), BindingFlags.Static | BindingFlags.NonPublic)!
#pragma warning restore S3011
                .MakeGenericMethod(entityType.ClrType);

            method.Invoke(null, [modelBuilder]);

        }
    }
    
    internal static void ApplyGlobalSoftDeleteFilter<TEntity>(ModelBuilder modelBuilder) where TEntity : class, ISoftDeletable
    {
        modelBuilder.Entity<TEntity>().HasQueryFilter(e => !e.IsDeleted);
    }
}
