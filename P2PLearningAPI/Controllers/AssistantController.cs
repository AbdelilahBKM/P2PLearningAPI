using Microsoft.AspNetCore.Mvc;
using P2PLearningAPI.DTOsOutput;
using P2PLearningAPI.Interfaces;

namespace P2PLearningAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssistantController : Controller
    {
        private readonly IAssistantInterface _simularityRepository;
        public AssistantController(IAssistantInterface simularityRepository)
        {
            _simularityRepository = simularityRepository;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<SimularityDTO>))]
        public IActionResult GetSimularities()
        {
            var simularities = _simularityRepository.GetSimularities();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(simularities);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(200, Type = typeof(SimularityDTO))]
        [ProducesResponseType(404)]
        public IActionResult GetSimularity(long id)
        {
            var simularity = _simularityRepository.GetSimularityById(id);
            if (simularity == null || !ModelState.IsValid)
                return NotFound();
            return Ok(simularity);
        }
        [HttpGet]
        [Route("Question/{questionId}")]
        [ProducesResponseType(200, Type = typeof(SimularityDTO))]
        [ProducesResponseType(404)]
        public IActionResult GetSimularityByQuestionId(long questionId)
        {
            var simularity = _simularityRepository.GetSimularity(questionId);
            if (simularity == null || !ModelState.IsValid)
                return NotFound();
            return Ok(simularity);
        }
        [HttpGet]
        [Route("SuggestedAnswer/{questionId}")]
        [ProducesResponseType(200, Type = typeof(SuggestedAnswerDTO))]
        [ProducesResponseType(404)]
        public IActionResult GetSuggestedAnswer(long questionId)
        {
            var suggestedAnswer = _simularityRepository.GetSuggestedAnswer(questionId);
            if (suggestedAnswer == null || !ModelState.IsValid)
                return NotFound();
            return Ok(suggestedAnswer);
        }

    }
}
