using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizBuilder.Domain.Question;
using QuizBuilder.Domain.Quiz;

namespace QuizBuilder.Infrastructure.Configuration;

internal sealed class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.ToTable("questions");
        
        builder.HasKey(q => q.Id);
        
        builder.Property(q => q.Number)
            .HasConversion(
                number => number.Value,
                number => new QuestionNumber(number));
        
        builder.Property(x => x.Description)
            .HasMaxLength(100)
            .IsRequired()
            .HasConversion(
                question => question.Value,
                question => new Description(question));

        builder.HasOne<Quiz>()
            .WithMany()
            .HasForeignKey(q => q.QuizId)
            .IsRequired();
    }
}
