namespace P2PLearningAPI.DTOsOutput
{
    public class QuestionDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsAnswered { get; set; }
        public DateTime PostedAt { get; set; }

        public UserMiniDTO PostedBy { get; set; }
        public List<AnswerDTO> Answers { get; set; } = new();
    }

}
