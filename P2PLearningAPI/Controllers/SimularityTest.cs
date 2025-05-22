using Microsoft.AspNetCore.Mvc;
using P2PLearningAPI.DTOsOutput;
using P2PLearningAPI.Interfaces;

namespace P2PLearningAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimularityAnswer : Controller
    {
        private readonly IAssistantAnswerInterface _similarityTestService;
        public SimularityAnswer(IAssistantAnswerInterface similarityTestService)
        {
            _similarityTestService = similarityTestService;
        }

        [HttpPost("similarity")]
        public async Task<IActionResult> GetSimilarityScores([FromBody] MiniQuestionDTO query)
        {
            if (query == null)
            {
                return BadRequest("Query cannot be null.");
            }
            var similarityScores = await _similarityTestService.GetSimilarityScoresAsync(query);
            return Ok(similarityScores);
        }
    }
}
