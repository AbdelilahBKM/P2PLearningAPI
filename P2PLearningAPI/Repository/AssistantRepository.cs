using Microsoft.EntityFrameworkCore;
using P2PLearningAPI.Data;
using P2PLearningAPI.DTOsOutput;
using P2PLearningAPI.Interfaces;

namespace P2PLearningAPI.Repository
{
    public class AssistantRepository: IAssistantInterface
    {
        private readonly P2PLearningDbContext _context;
        public AssistantRepository(P2PLearningDbContext context)
        {
            _context = context;
        }
        public ICollection<SimularityDTO> GetSimularities()
        {
            return _context.Simularities
                .Include(s => s.Question)
                    .ThenInclude(q => q.PostedBy)
                .Include(s => s.SimularityQuestions)
                .ThenInclude(sq => sq.Question)
                .Select(s => SimularityDTO.FromSimularity(s))
                .ToList();
        }
        public SimularityDTO? GetSimularityById(long id)
        {
            return _context.Simularities
                .Include(s => s.Question)
                .Include(s => s.SimularityQuestions)
                .ThenInclude(sq => sq.Question)
                .Where(s => s.Id == id)
                .Select(s => SimularityDTO.FromSimularity(s))
                .FirstOrDefault();
        }

        public SimularityDTO? GetSimularity(long QuestionId)
        {
            return _context.Simularities
                .Include(s => s.Question)
                .Include(s => s.SimularityQuestions)
                .ThenInclude(sq => sq.Question)
                .Where(s => s.QuestionId == QuestionId)
                .Select(s => SimularityDTO.FromSimularity(s))
                .FirstOrDefault();
        }

        public SuggestedAnswerDTO? GetSuggestedAnswer(long QuestionId)
        {
            return _context.SuggestedAnswers
                .Include(s => s.Question)
                .Where(s => s.QuestionId == QuestionId)
                .Select(s => new SuggestedAnswerDTO
                {
                    Id = s.Id,
                    Answer = s.Answer,
                    QuestionId = s.QuestionId
                })
                .FirstOrDefault();
        }
    }
}
