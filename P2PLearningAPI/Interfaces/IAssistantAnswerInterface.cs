using P2PLearningAPI.DTOsOutput;

namespace P2PLearningAPI.Interfaces
{
    public interface IAssistantAnswerInterface
    {
        public Task<List<CandidateScore>> GetSimilarityScoresAsync(MiniQuestionDTO query);
        public Task<SimularityDTO?> CreateSimularityAnswerAsync(MiniQuestionDTO query);
        public Task<SuggestedAnswerDTO?> CreateSuggestedAnswer(MiniQuestionDTO query);
    }
}
