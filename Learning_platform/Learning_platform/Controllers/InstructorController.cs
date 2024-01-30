using Learning_platform.DTO;
using Learning_platform.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Learning_platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InstructorController(ApplicationDbContext context)
        {
            _context = context;
        }

        //[HttpPost("addinstructor")]
        //public async Task<IActionResult> AddInstructor([FromForm] InstructorDTO instructorDTO)
        //{
        //    if (instructorDTO == null)
        //    {
        //        return BadRequest("Invalid instructor data.");
        //    }

        //    if (instructorDTO.Imageprofile != null)
        //    {
        //        string[] allowedExtensions = { ".png", ".jpg" };
        //        if (!allowedExtensions.Contains(Path.GetExtension(instructorDTO.Imageprofile.FileName).ToLower()))
        //        {
        //            return BadRequest("Only .png and .jpg images are allowed!");
        //        }

        //        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "instructorImage");

        //        var uniqueFileName = Guid.NewGuid().ToString() + "_" + instructorDTO.Imageprofile.FileName;
        //        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await instructorDTO.Imageprofile.CopyToAsync(fileStream);
        //        }

        //        instructorDTO.Photo = Path.Combine("instructorImage", uniqueFileName);
        //    }
        //    else
        //    {
        //        return BadRequest("Profile image is required.");
        //    }

        //    var instructor = new Instructor
        //    {
        //        Name = instructorDTO.Name,
        //        Photo = instructorDTO.Photo,
        //        Description = instructorDTO.Description,
        //    };

        //    _context.Instructors.Add(instructor);
        //    await _context.SaveChangesAsync();

        //    return Ok("Instructor added successfully.");
        //}

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateInstructor(int id, [FromForm] InstructorDTO instructorDTO)
        //{
        //    var instructorToUpdate = await _context.Instructors.FindAsync(id);

        //    if (instructorToUpdate == null)
        //    {
        //        return NotFound();
        //    }

        //    if (instructorDTO.Imageprofile != null)
        //    {
        //        if (!string.IsNullOrEmpty(instructorToUpdate.Photo))
        //        {
        //            var existingFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", instructorToUpdate.Photo);
        //            if (System.IO.File.Exists(existingFilePath))
        //            {
        //                System.IO.File.Delete(existingFilePath);
        //            }
        //        }

        //        string[] allowedExtensions = { ".png", ".jpg" };
        //        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "instructorImage");

        //        if (!allowedExtensions.Contains(Path.GetExtension(instructorDTO.Imageprofile.FileName).ToLower()))
        //        {
        //            return BadRequest("Only .png and .jpg images are allowed!");
        //        }

        //        var uniqueFileName = Guid.NewGuid().ToString() + "_" + instructorDTO.Imageprofile.FileName;
        //        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await instructorDTO.Imageprofile.CopyToAsync(fileStream);
        //        }

        //        /*/*/*/*/*instructorDTO*/*/*/*/*/.Photo = Path.Combine("instructorImage", uniqueFileName);
        //    }

        //    instructorToUpdate.Name = instructorDTO.Name;
        //    instructorToUpdate.Photo = instructorDTO.Imageprofile;
        //    instructorToUpdate.Description = instructorDTO.Description;

        //    await _context.SaveChangesAsync();

        //    return Ok("Instructor updated successfully.");
        //}


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInstructor(int id)
        {
            var instructorToDelete = await _context.Instructors.FindAsync(id);

            if (instructorToDelete == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(instructorToDelete.Photo))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", instructorToDelete.Photo);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Instructors.Remove(instructorToDelete);
            await _context.SaveChangesAsync();

            return Ok("Instructor deleted successfully.");
        }
    }
}