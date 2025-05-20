using Microsoft.EntityFrameworkCore;
using P2PLearningAPI.Data;
using P2PLearningAPI.DTOsOutput;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;
using P2PLearningAPI.Services;

namespace P2PLearningAPI.Repository
{
    public class SimularityAnswerRepository: ISimularityAnswerInterface
    {
        private readonly HttpClient _httpClient;
        private readonly SimilarityService _similarityService;
        private readonly P2PLearningDbContext _context;
        public SimularityAnswerRepository(HttpClient httpClient, P2PLearningDbContext context)
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

        public async Task CreateSimularityAnswerAsync(MiniQuestionDTO query)
        {
            // Get the main question entity
            var mainQuestion = await _context.Questions
                .FirstOrDefaultAsync(q => q.Id == query.Id);

            if (mainQuestion == null) return;

            // Get candidate questions (excluding current question)
            var candidates = await _context.Questions
                .Where(q => q.Id != query.Id)
                .OrderByDescending(q => q.Created_at)
                .ToListAsync();

            if (!candidates.Any())
                return;

            // Get similarity scores
            var candidateIds = candidates.Select(c => c.Id).ToList();
            var candidateDTOs = candidates.Select(c => new MiniQuestionDTO
            {
                Id = c.Id,
                Title = c.Title,
                Content = c.Content
            }).ToList();

            var similarityScores = await _similarityService
                .GetSimilarityScoresAsync(query, candidateIds, candidateDTOs);

            // Filter and take top 3 relevant matches
            var accurateScores = similarityScores
                .Where(cs => cs.Score >= 0.7)
                .OrderByDescending(cs => cs.Score)
                .Take(3)
                .ToList();

            if (!accurateScores.Any())
                return;

            // Create new Simularity record
            var similarity = new Simularity(mainQuestion)
            {
                UpdatedAt = DateTime.UtcNow
            };

            // Add candidate relationships
            foreach (var score in accurateScores)
            {
                var candidateQuestion = candidates.FirstOrDefault(c => c.Id == score.Id);
                if (candidateQuestion != null)
                {
                    similarity.SimularityQuestions.Add(new SimularityQuestion
                    {
                        Question = candidateQuestion,
                        Score = score.Score
                    });
                }
            }

            // Save to database
            _context.Simularities.Add(similarity);
            await _context.SaveChangesAsync();
        }
    }
}
