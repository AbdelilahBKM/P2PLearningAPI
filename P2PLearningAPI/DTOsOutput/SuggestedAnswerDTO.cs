using P2PLearningAPI.Models;

namespace P2PLearningAPI.DTOsOutput
{
    public class SuggestedAnswerDTO
    {
        public long Id { get; set; }
        public string Answer { get; set; } = string.Empty;
        public long QuestionId { get; set; }
    }
}
