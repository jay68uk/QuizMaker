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
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired()
            .HasConversion(
                quizName => quizName.Value,
                quizName => new QuizName(quizName));
        
        builder.Property(x => x.CreatedBy).IsRequired();
        
        builder.Property(x => x.CreatedDate).IsRequired();
        
        builder.Property(x => x.Status).IsRequired().HasConversion<int>();

        builder.Property(x => x.RanDate);

        builder.OwnsOne<QuizAccessCode>(q => q.AccessCode).ToTable("quiz_accessCode");
        
        builder.HasIndex(x => x.Id).IsUnique();
        
        builder.HasIndex(x => x.CreatedBy);
    }
}
