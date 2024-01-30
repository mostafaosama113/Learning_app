using Learning_platform.DTO;
using Learning_platform.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Learning_platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VoteController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllVotesForCourse(int courseId)
        {
            var votes = _context.Votes.Where(v => v.Course.Id == courseId).ToList();
            var voteDTOs = votes.Select(v => new VoteDTO
            {
                Value = v.Value
            }).ToList();
            return Ok(voteDTOs);
        }

        [HttpPost]
        public IActionResult AddVoteForCourse(int courseId, [FromBody] VoteDTO voteDTO)
        {
            if (voteDTO == null)
            {
                return BadRequest("Invalid vote data.");
            }

            var course = _context.Courses.Find(courseId);

            if (course == null)
            {
                return NotFound("Course not found.");
            }

            var vote = new Vote
            {
                Value = voteDTO.Value,
                Course = course
            };

            _context.Votes.Add(vote);
            _context.SaveChanges();

            return Ok("Vote added successfully.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateVoteForCourse(int courseId, int id, [FromBody] VoteDTO voteDTO)
        {
            var existingVote = _context.Votes.FirstOrDefault(v => v.Id == id && v.Course.Id == courseId);

            if (existingVote == null)
            {
                return NotFound("Vote not found.");
            }

            existingVote.Value = voteDTO.Value;

            _context.SaveChanges();

            return Ok("Vote updated successfully.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteVoteForCourse(int courseId, int id)
        {
            var voteToDelete = _context.Votes.FirstOrDefault(v => v.Id == id && v.Course.Id == courseId);

            if (voteToDelete == null)
            {
                return NotFound("Vote not found.");
            }

            _context.Votes.Remove(voteToDelete);
            _context.SaveChanges();

            return Ok("Vote deleted successfully.");
        }
    }
}
