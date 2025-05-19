using P2PLearningAPI.DTOsOutput;

namespace P2PLearningAPI.Interfaces
{
    public interface ISimularityTestInterface
    {
        public Task<List<CandidateScore>> GetSimilarityScoresAsync(MiniQuestionDTO query);
    }
}
