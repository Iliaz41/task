using System.ComponentModel.DataAnnotations;
using task.Models;

namespace task.Dtos
{
    public class CreateRoleDto
    {
        public string Status { get; set; }

        public IList<User>? Users { get; set; }
    }
}