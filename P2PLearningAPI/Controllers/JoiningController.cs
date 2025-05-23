﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2PLearningAPI.DTOs;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;

namespace P2PLearningAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JoiningController : ControllerBase
    {
        private readonly IJoiningInterface _joiningRepository;

        public JoiningController(IJoiningInterface joiningRepository)
        {
            _joiningRepository = joiningRepository;
        }

        // GET: api/Joining
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Joining>))]
        public IActionResult GetJoinings()
        {
            var joinings = _joiningRepository.GetJoinings();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(joinings);
        }

        // GET: api/Joining/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Joining))]
        [ProducesResponseType(404)]
        public IActionResult GetJoining(long id)
        {
            var joining = _joiningRepository.GetJoining(id);
            if (joining == null)
                return NotFound();

            return Ok(joining);
        }

        // GET: api/Joining/{userId}/{discussionId}
        [HttpGet("{userId}/{discussionId}")]
        [ProducesResponseType(200, Type = typeof(Joining))]
        [ProducesResponseType(404)]
        public IActionResult GetJoining(string userId, long discussionId)
        {
            var joining = _joiningRepository.GetJoining(userId, discussionId);
            if (joining == null)
                return NotFound();

            return Ok(joining);
        }

        // GET: api/Joining/ByUser/{userId}
        [HttpGet("ByUser/{userId}")]
        [ProducesResponseType(200, Type = typeof(ICollection<Joining>))]
        [ProducesResponseType(404)]
        public IActionResult GetJoiningsByUser(string userId)
        {
            var joinings = _joiningRepository.GetJoiningsByUser(userId);

            return Ok(joinings);
        }

        // GET: api/Joining/ByDiscussion/{discussionId}
        [HttpGet("ByDiscussion/{discussionId}")]
        [ProducesResponseType(200, Type = typeof(ICollection<Joining>))]
        [ProducesResponseType(404)]
        public IActionResult GetJoiningsByDiscussion(long discussionId)
        {
            var joinings = _joiningRepository.GetJoiningsByDiscussion(discussionId);
            if (joinings == null || !joinings.Any())
                return NotFound();

            return Ok(joinings);
        }

        // POST: api/Joining
        [Authorize]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Joining))]
        [ProducesResponseType(400)]
        public IActionResult CreateJoining([FromBody] JoiningCreateDTO joining)
        {
            if (joining == null)
                return BadRequest("Invalid joining data.");
            try
            {
                var authHeader = Request.Headers["Authorization"]!;
                string token = authHeader.ToString().Split(" ")[1];
                var createdJoining = _joiningRepository.CreateJoining(
                    new Joining(
                        joining.userId,
                        joining.discussionId
                        ),
                    token
                    );

                return CreatedAtAction(nameof(GetJoining), new { id = createdJoining.Id }, createdJoining);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Joining/{id}
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteJoining(long id)
        {
            try
            {
                var authHeader = Request.Headers["Authorization"]!;
                string token = authHeader.ToString().Split(" ")[1];
                var success = _joiningRepository.DeleteJoining(id, token);
                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
