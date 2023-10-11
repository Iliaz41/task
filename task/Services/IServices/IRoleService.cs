using task.Dtos;
using task.Models;

namespace task.Services.IServices
{
    public interface IRoleService
    {
        Task<List<Role>> GetAll();
        Task<Role> GetById(long id);
        Task<Role> Create(Role entity);
        Task<Role> Update(Role entity);
        Task Delete(long id);
    }
}