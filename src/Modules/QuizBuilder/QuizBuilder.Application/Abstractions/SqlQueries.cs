namespace QuizBuilder.Application.Abstractions;

internal static class SqlQueries
{
    public const string GetQuizById =
        """
        SELECT 
            q.Id, q.Name, qaC.access_code AS AccessCode, q.created_date AS CreatedDate, q.ran_date AS RanDate, q.created_by AS CreatedBy, q.Status,
            qu.Id AS QuestionId, qu.Number AS QuestionNumber, qu.Description AS QuestionDescription
        FROM quizzes q
        LEFT JOIN questions qu ON q.Id = qu.quiz_id
        LEFT JOIN public."quiz_accessCode" qaC ON q.id = qaC.id
        WHERE q.Is_Deleted = FALSE AND q.Id = @QuizId
        """;
    
}
