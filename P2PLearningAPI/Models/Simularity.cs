namespace P2PLearningAPI.Models
{
    public class Simularity
    {
        public long Id { get; set; }
        public long QuestionId { get; set; }
        public Question Question { get; set; } = null!;

        // Many-to-many with join entity
        public List<SimularityQuestion> SimularityQuestions { get; set; } = new();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Simularity() { }

        public Simularity(Question question)
        {
            Question = question;
        }
    }
}
