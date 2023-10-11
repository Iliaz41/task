using System.ComponentModel.DataAnnotations;
using task.Models;

namespace task.Dtos
{
    public class UserDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Email { get; set; }

        public IList<Role>? Roles { get; set; }
    }
}
