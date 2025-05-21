using P2PLearningAPI.Models;

namespace P2PLearningAPI.DTOsOutput
{
    public class SimularityDTO
    {
        public long Id { get; set; }
        public long QuestionId { get; set; }
        public QuestionDTO Question { get; set; } = null!;

        // Many-to-many with join entity
        public List<SimularityQuestionDTO> SimularityQuestions { get; set; } = new();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public SimularityDTO(
            long id,
            long questionId,
            QuestionDTO question,
            List<SimularityQuestionDTO> simularityQuestions,
            DateTime createdAt,
            DateTime updatedAt
        )
        {
            Id = id;
            QuestionId = questionId;
            Question = question;
            SimularityQuestions = simularityQuestions;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
        public static SimularityDTO FromSimularity(Simularity simularity)
        {
            return new SimularityDTO(
                simularity.Id,
                simularity.QuestionId,
                QuestionDTO.FromQuestion(simularity.Question),
                simularity.SimularityQuestions.Select(sq => SimularityQuestionDTO.FromSimularityQuestion(sq)).ToList(),
                simularity.CreatedAt,
                simularity.UpdatedAt
                );
        }
    }
}
