using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace task.Models
{
    public class User
    {
        [Key]
        public long Id { get; set; }

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

        [JsonIgnore]
        public IList<Role>? Roles { get; set; } = new List<Role>();
    }
}
