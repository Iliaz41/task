using System.ComponentModel.DataAnnotations;

namespace task.Dtos
{
    public class UpdateUserDto
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "The age must be between 1 and 200.")]
        public int Age { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
    }
}
