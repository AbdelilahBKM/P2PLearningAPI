using Microsoft.AspNetCore.Mvc;
using P2PLearningAPI.DTOsOutput;
using P2PLearningAPI.Interfaces;

namespace P2PLearningAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimularityTest : Controller
    {
        private readonly ISimularityTestInterface _similarityTestService;
        public SimularityTest(ISimularityTestInterface similarityTestService)
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
