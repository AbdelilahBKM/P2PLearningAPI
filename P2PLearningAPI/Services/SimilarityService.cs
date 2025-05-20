using P2PLearningAPI.DTOsOutput;

namespace P2PLearningAPI.Services
{
    public class SimilarityService
    {
        private readonly HttpClient _httpClient;
        public SimilarityService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<CandidateScore>> GetSimilarityScoresAsync(MiniQuestionDTO query, List<long> candidateIds, List<MiniQuestionDTO> candidates)
        {
            var request = new SimilarityRequest
            {
                Question = query,
                CandidateIds = candidateIds,
                Candidates = candidates
            };

            var response = await _httpClient.PostAsJsonAsync(
                "http://127.0.0.1:8000/cross-encoder/similarity",
                request
            );
            response.EnsureSuccessStatusCode();

            var similarityResponse = await response.Content.ReadFromJsonAsync<SimilarityResponse>();
            return similarityResponse?.Results ?? new List<CandidateScore>();
        }
    }
}
