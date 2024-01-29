using Learning_platform.DTO;
using Learning_platform.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Learning_platform.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("addcategor")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDTO categoryDTO)
        {
            if (categoryDTO == null)
            {
                return BadRequest("Invalid term data.");
            }
            var category = new Category
            {
                Name = categoryDTO.Name
            };
            _context.Category.Add(category);
            await _context.SaveChangesAsync();

            return Ok("category added successfully.");
        }

        [HttpPut("updatecategory/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDTO updatedCategoryDTO)
        {
            var existingCategory = await _context.Category.FindAsync(id);

            if (existingCategory == null)
            {
                return NotFound("Category not found.");
            }

            existingCategory.Name = updatedCategoryDTO.Name;

            await _context.SaveChangesAsync();

            return Ok("Category updated successfully.");
        }

        [HttpDelete("deletecategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Category.FindAsync(id);

            if (category == null)
            {
                return NotFound("Category not found.");
            }

            _context.Category.Remove(category);
            await _context.SaveChangesAsync();

            return Ok("Category deleted successfully.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _context.Category.FindAsync(id);

            if (category == null)
            {
                return NotFound("Category not found.");
            }

            return Ok(category);
        }
        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var categories = _context.Category.ToList();
            return Ok(categories);
        }
    }
}
