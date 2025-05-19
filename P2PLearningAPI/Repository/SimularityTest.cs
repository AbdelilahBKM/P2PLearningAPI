using P2PLearningAPI.Data;
using P2PLearningAPI.DTOsOutput;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Services;

namespace P2PLearningAPI.Repository
{
    public class SimularityTest: ISimularityTestInterface
    {
        private readonly HttpClient _httpClient;
        private readonly SimilarityService _similarityService;
        private readonly P2PLearningDbContext _context;
        public SimularityTest(HttpClient httpClient, P2PLearningDbContext context)
        {
            _httpClient = httpClient;
            _context = context;
            _similarityService = new SimilarityService(httpClient);
        }

        public Task<List<CandidateScore>> GetSimilarityScoresAsync(MiniQuestionDTO query)
        {
            var candidateIds = _context.Questions
                .Where(q => q.Id != query.Id)
                .OrderByDescending(q => q.Created_at)
                .Select(q => q.Id)
                .ToList();

            var candidates = _context.Questions
                .Where(q => candidateIds.Contains(q.Id))
                .OrderByDescending(q => q.Created_at)
                .Select(q => new MiniQuestionDTO
                {
                    Id = q.Id,
                    Title = q.Title,
                    Content = q.Content
                })
                .ToList();
            return _similarityService.GetSimilarityScoresAsync(query, candidateIds, candidates);
        }
    }
}
