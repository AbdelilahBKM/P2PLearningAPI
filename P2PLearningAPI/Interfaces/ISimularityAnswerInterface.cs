using P2PLearningAPI.DTOsOutput;

namespace P2PLearningAPI.Interfaces
{
    public interface ISimularityAnswerInterface
    {
        public Task<List<CandidateScore>> GetSimilarityScoresAsync(MiniQuestionDTO query);
        public Task CreateSimularityAnswerAsync(MiniQuestionDTO query);
    }
}
