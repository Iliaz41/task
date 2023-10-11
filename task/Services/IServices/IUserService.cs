using System.Net;
using task.Models;

namespace task.Services.IServices
{
    public interface IUserService
    {
        Task<User> GetById(long id);
        Task<User> Create(CreateUser entity);
        Task<User> Update(User entity);
        Task Delete(long id);
        Task<User> AddRole(long userId, long roleId);
        Task<User> RemoveRole(long userId, long roleId);
        Task<List<User>> SortByCriteria(int? page, Criteria criteria);
        Task<List<User>> FilterByCriteria(int? page, Criteria criteria);
        Task<string> CreateToken(Auth entity);
        Task<string> Login(Auth entity);
    }
}
