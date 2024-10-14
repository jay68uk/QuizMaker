using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizBuilder.Domain.Quiz;
using QuizBuilder.Domain.QuizAccessCode;

namespace QuizBuilder.Infrastructure.Configuration;

internal sealed class QuizConfiguration : IEntityTypeConfiguration<Quiz>
{
    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        builder.ToTable("quizzes");
        
        builder.HasKey(q => q.Id);
        
        builder.Property(q => q.Name)
            .HasMaxLength(100)
            .IsRequired()
            .HasConversion(
                quizName => quizName.Value,
                quizName => new QuizName(quizName));
        
        builder.Property(q => q.CreatedBy).IsRequired();
        
        builder.Property(q => q.CreatedDate).IsRequired();
        
        builder.Property(q => q.Status).IsRequired().HasConversion<int>();

        builder.Property(q => q.RanDate);
        
        builder.Property(q => q.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(q => q.DeletedDate);

        builder.OwnsOne<QuizAccessCode>(q => q.AccessCode).ToTable("quiz_accessCode");
        
        builder.HasIndex(q => q.CreatedBy);
        
        builder.HasMany(q => q.QuestionsNavigation)
            .WithOne()
            .HasForeignKey(q => q.QuizId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
