using System.ComponentModel.DataAnnotations;

namespace task.Dtos
{
    public class AuthDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required]
        [MinLength(5)]
        public string Password { get; set; }
    }
}
