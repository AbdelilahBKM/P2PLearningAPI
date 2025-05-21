using P2PLearningAPI.DTOsOutput;
using P2PLearningAPI.Models;

public class SimularityQuestionDTO
{
    public long SimularityId { get; set; }
    public long QuestionId { get; set; }

    public QuestionDTO Question { get; set; } = null!;
    public double Score { get; set; }

    public SimularityQuestionDTO(
        long simularityId,
        long questionId,
        QuestionDTO question,
        double score
    )
    {
        SimularityId = simularityId;
        QuestionId = questionId;
        Question = question;
        Score = score;
    }

    public static SimularityQuestionDTO FromSimularityQuestion(SimularityQuestion simularityQuestion)
    {
        return new SimularityQuestionDTO(
            simularityQuestion.SimularityId,
            simularityQuestion.QuestionId,
            QuestionDTO.FromQuestion(simularityQuestion.Question),
            simularityQuestion.Score
        );
    }
}
