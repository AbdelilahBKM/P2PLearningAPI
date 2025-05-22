namespace P2PLearningAPI.Models
{
    public class SuggestedAnswer
    {
        public long Id { get; set; }
        public string Answer { get; set; } = string.Empty;
        public long QuestionId { get; set; }
        public Question Question { get; set; } = new Question();
    }
}
