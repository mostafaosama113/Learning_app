using System.ComponentModel.DataAnnotations;

namespace Learning_platform.DTO
{
    public class InstructorDTO
    {
        public string Name { get; set; }
        public IFormFile Imageprofile { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
    }
}
