﻿using Learning_platform.DTO;
using Learning_platform.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
namespace Learning_platform.Controllers
{
    [ApiController]
    [Route("api/courses")]
    public class CourseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        IWebHostEnvironment webHostEnvironment;
        public CourseController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult GetCourses()
        {
            var courses = _context.Courses.ToList();
            var courseDTOs = courses.Select(c => new CourseDTO {
                Id = c.Id,
                Name = c.Name, 
                Description = c.Description, 
            }).ToList();
            return Ok(courseDTOs);
        }
        [HttpGet("{id}")]
        public IActionResult GetCourseById(int id)
        {
            var course = _context.Courses.Find(id);

            if (course == null)
            {
                return NotFound();
            }

            var courseDTO = new CourseDTO { Id = course.Id, Name = course.Name, Description = course.Description };
            return Ok(courseDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse([FromForm] AddCourseDTO courseDTO)
        {
            if (courseDTO == null)
            {
                return BadRequest("Invalid course data.");
            }

            var category = await _context.Category.FindAsync(courseDTO.Category_Id);

            if (category == null)
            {
                return BadRequest($"Category with id '{courseDTO.Category_Id}' not found.");
            }

            if (_context.Courses.Any(c => c.Name == courseDTO.Name))
            {
                return BadRequest($"Course with name '{courseDTO.Name}' already exists.");
            }

            var uniqueFileName = await SaveFile(courseDTO.ImageOfCertificate, "certificates");

            var course = new Course
            {
                Name = courseDTO.Name,
                Description = courseDTO.Description,
                ImageOfCertificate = uniqueFileName,
                Category = category
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return Ok("Course added successfully.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromForm] AddCourseDTO courseUpdateDTO)
        {
            var existingCourse = await _context.Courses.FindAsync(id);

            if (existingCourse == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(courseUpdateDTO.Category_Id);

            if (category == null)
            {
                return BadRequest($"Category with id '{courseUpdateDTO.Category_Id}' not found.");
            }

            if (_context.Courses.Any(c => c.Name == courseUpdateDTO.Name && c.Id != id))
            {
                return BadRequest($"Course with name '{courseUpdateDTO.Name}' already exists.");
            }

            var uniqueFileName = await SaveFile(courseUpdateDTO.ImageOfCertificate, "certificates");

            existingCourse.Name = courseUpdateDTO.Name;
            existingCourse.Description = courseUpdateDTO.Description;
            existingCourse.ImageOfCertificate = uniqueFileName;
            existingCourse.Category = category;

            await _context.SaveChangesAsync();

            return Ok("Course Updated successfully.");
        }

        private async Task<string> SaveFile(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
            {
                return null; // Handle the case where no file is provided
            }

            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, folder);

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return uniqueFileName;
        }

        // DELETE: api/courses/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return Ok("Course delete successfully.");
        }
    }
}