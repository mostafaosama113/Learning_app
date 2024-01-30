using Learning_platform.DTO;
using Learning_platform.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Learning_platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LessonController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("addlesson")]
        public async Task<IActionResult> AddLesson([FromForm] LessonDTO lessonDTO)
        {
            if (lessonDTO == null)
            {
                return BadRequest("Invalid lesson data.");
            }

            var course = await _context.Courses.FindAsync(lessonDTO.CourseId);

            if (course == null)
            {
                return BadRequest($"Course with id '{lessonDTO.CourseId}' not found.");
            }
            if (lessonDTO.VideoFile != null)
            {
                string[] allowedExtensions = { ".mp4", ".avi", ".mkv" }; 
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "videos");
                if (!allowedExtensions.Contains(Path.GetExtension(lessonDTO.VideoFile.FileName).ToLower()))
                {
                    return BadRequest("Only .mp4, .avi, and .mkv videos are allowed!");
                }

                var uniqueFileName = await SaveFile(lessonDTO.VideoFile, uploadsFolder);

                var lesson = new Lesson
                {
                    Name = lessonDTO.Name,
                    Description = lessonDTO.Description,
                    Video = uniqueFileName,
                    Price = lessonDTO.Price,
                    Course = course
                };

                _context.Lessons.Add(lesson);
                await _context.SaveChangesAsync();

                return Ok("Lesson added successfully.");
            }
            return BadRequest();
        }

        [HttpPut("updatelesson/{id}")]
        public async Task<IActionResult> UpdateLesson(int id, [FromForm] LessonDTO lessonDTO)
        {
            var lessonToUpdate = await _context.Lessons.FindAsync(id);

            if (lessonToUpdate == null)
            {
                return NotFound($"Lesson with id '{id}' not found.");
            }

            if (lessonDTO.VideoFile != null)
            {
                string[] allowedExtensions = { ".mp4", ".avi", ".mkv" };
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "videos");

                if (!allowedExtensions.Contains(Path.GetExtension(lessonDTO.VideoFile.FileName).ToLower()))
                {
                    return BadRequest("Only .mp4, .avi, and .mkv videos are allowed!");
                }

                var uniqueFileName = await SaveFile(lessonDTO.VideoFile, uploadsFolder);
                lessonToUpdate.Video = uniqueFileName;
            }

            lessonToUpdate.Name = lessonDTO.Name;
            lessonToUpdate.Description = lessonDTO.Description;
            lessonToUpdate.Price = lessonDTO.Price;

            await _context.SaveChangesAsync();

            return Ok("Lesson updated successfully.");
        }

        [HttpDelete("deletelesson/{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            var lessonToDelete = await _context.Lessons.FindAsync(id);

            if (lessonToDelete == null)
            {
                return NotFound($"Lesson with id '{id}' not found.");
            }

            _context.Lessons.Remove(lessonToDelete);
            await _context.SaveChangesAsync();

            return Ok("Lesson deleted successfully.");
        }

        private async Task<string> SaveFile(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
            {
                return null; 
            }
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder);

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return Path.Combine(folder, uniqueFileName);
        }
    }
}