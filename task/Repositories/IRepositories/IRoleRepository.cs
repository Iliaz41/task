using task.Models;

namespace task.Repositories.IRepositories
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetAll();
        Task<Role> GetById(long id);
        Task<Role> Create(Role entity);
        Task<Role> Update(Role entity);
        Task Delete(Role entity);
        bool IsRoleExists(string status);
    }
}
