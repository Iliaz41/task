using task.Models;

namespace task.Repositories.IRepositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAll();
        User GetById(long id);
        Task<User> Create(User entity);
        Task<User> Update(User entity);
        Task Delete(User entity);
        bool IsUserExists(string email);
    }
}
