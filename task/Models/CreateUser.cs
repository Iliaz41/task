using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace task.Models
{
    public class CreateUser
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1, 200, ErrorMessage = "The age must be between 1 and 200.")]
        public int Age { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required]
        [MinLength(5)]
        public string Password { get; set; }

        public long RoleId { get; set; }
    }
}
