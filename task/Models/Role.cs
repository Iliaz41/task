using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace task.Models
{
    public class Role
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Status { get; set; }

        [JsonIgnore]
        public IList<User>? Users { get; set; } = new List<User>();
    }
}
