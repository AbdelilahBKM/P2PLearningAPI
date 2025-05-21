using Microsoft.AspNetCore.Mvc;
using P2PLearningAPI.DTOsOutput;
using P2PLearningAPI.Interfaces;

namespace P2PLearningAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimilarQuestions : Controller
    {
        private readonly ISimularityInterface _simularityRepository;
        public SimilarQuestions(ISimularityInterface simularityRepository)
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

    }
}
