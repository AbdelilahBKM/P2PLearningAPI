namespace P2PLearningAPI.DTOsOutput
{
    public class SimilarityRequest
    {
        public MiniQuestionDTO Question { get; set; }
        public List<long> CandidateIds { get; set; }
        public List<MiniQuestionDTO> Candidates { get; set; }
    }
    public class CandidateScore
    {
        public int Id { get; set; }
        public float Score { get; set; }
    }
    public class SimilarityResponse
    {
        public List<CandidateScore> Results { get; set; }
    }
}
