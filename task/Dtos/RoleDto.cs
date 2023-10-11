using System.ComponentModel.DataAnnotations;
using task.Models;

namespace task.Dtos
{
    public class RoleDto
    {
        public long Id { get; set; }

        public string Status { get; set; }

        public IList<User>? Users { get; set; }
    }
}
