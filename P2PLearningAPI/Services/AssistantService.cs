using P2PLearningAPI.DTOsOutput;

namespace P2PLearningAPI.Services
{
    public class SuggestedAnswerOutput
    {
        public string Answer { get; set; } = string.Empty;
    }
    public class AssistantService
    {
        private readonly HttpClient _httpClient;
        public AssistantService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<CandidateScore>> GetSimilarityScoresAsync(
        MiniQuestionDTO query,
        List<long> candidateIds,
        List<MiniQuestionDTO> candidates
        )
        {
            // Exclude the query itself from the candidates list
            var filteredCandidates = candidates
                .Where(c => c.Id != query.Id)
                .ToList();

            var filteredCandidateIds = candidateIds
                .Where(id => id != query.Id)
                .ToList();

            var request = new SimilarityRequest
            {
                Question = query,
                CandidateIds = filteredCandidateIds,
                Candidates = filteredCandidates
            };

            var response = await _httpClient.PostAsJsonAsync(
                "http://127.0.0.1:8000/cross-encoder/similarity",
                request
            );
            response.EnsureSuccessStatusCode();

            var similarityResponse = await response.Content.ReadFromJsonAsync<SimilarityResponse>();
            return similarityResponse?.Results ?? new List<CandidateScore>();
        }

        public async Task<SuggestedAnswerDTO> GetSuggestedAnswerAsync(MiniQuestionDTO question)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "http://127.0.0.1:8000/assistant/assist",
                question
            );
            response.EnsureSuccessStatusCode();
            var answer = await response.Content.ReadFromJsonAsync<SuggestedAnswerOutput>();
            return new SuggestedAnswerDTO
            {
                Answer = answer?.Answer ?? string.Empty,
                QuestionId = question.Id
            };
        }

    }
}
