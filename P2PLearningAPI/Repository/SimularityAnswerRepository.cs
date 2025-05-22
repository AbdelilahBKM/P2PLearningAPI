using Microsoft.EntityFrameworkCore;
using P2PLearningAPI.Data;
using P2PLearningAPI.DTOsOutput;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;
using P2PLearningAPI.Services;

namespace P2PLearningAPI.Repository
{
    public class SimularityAnswerRepository: IAssistantAnswerInterface
    {
        private readonly HttpClient _httpClient;
        private readonly AssistantService _assistantService;
        private readonly P2PLearningDbContext _context;
        public SimularityAnswerRepository(HttpClient httpClient, P2PLearningDbContext context)
        {
            _httpClient = httpClient;
            _context = context;
            _assistantService = new AssistantService(httpClient);
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
            return _assistantService.GetSimilarityScoresAsync(query, candidateIds, candidates);
        }

        public async Task<SimularityDTO?> CreateSimularityAnswerAsync(MiniQuestionDTO query)
        {
            // Get the main question entity
            var mainQuestion = await _context.Questions
                .FirstOrDefaultAsync(q => q.Id == query.Id);

            if (mainQuestion == null)
                throw new ArgumentException("Question not found.");

            // Get candidate questions (excluding current question)
            var candidates = await _context.Questions
                .Where(q => q.Id != query.Id)
                .OrderByDescending(q => q.Created_at)
                .ToListAsync();

            if (!candidates.Any())
                return null;

            // Get similarity scores
            var candidateIds = candidates.Select(c => c.Id).ToList();
            var candidateDTOs = candidates.Select(c => new MiniQuestionDTO
            {
                Id = c.Id,
                Title = c.Title,
                Content = c.Content
            }).ToList();

            var similarityScores = await _assistantService
                .GetSimilarityScoresAsync(query, candidateIds, candidateDTOs);

            // Filter and take top 3 relevant matches
            var accurateScores = similarityScores
                .Where(cs => cs.Score >= 0.56)
                .OrderByDescending(cs => cs.Score)
                .Take(3)
                .ToList();

            if (!accurateScores.Any())
                return null;

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
            return SimularityDTO.FromSimularity(similarity);
        }

        public async Task<SuggestedAnswerDTO?> CreateSuggestedAnswer(MiniQuestionDTO query)
        {
            var suggestedAnswer = await _assistantService.GetSuggestedAnswerAsync(query);
            var question = await _context.Questions
                .FirstOrDefaultAsync(q => q.Id == query.Id);
            if (question != null)
            {
                var answer = new SuggestedAnswer
                {
                    Question = question,
                    QuestionId = question.Id,
                    Answer = suggestedAnswer.Answer
                };
                _context.SuggestedAnswers.Add(answer);
                await _context.SaveChangesAsync();
            }
            return suggestedAnswer;
        }
    }
}
