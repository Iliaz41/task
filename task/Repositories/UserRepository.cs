using Microsoft.EntityFrameworkCore;
using task.Data;
using task.Models;
using task.Repositories.IRepositories;

namespace task.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserRepository(ApplicationDbContext dbContext) 
        { 
            _dbContext = dbContext; 
        }
        public async Task<User> Create(User entity)
        {
            var user = await _dbContext.Users.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return user.Entity;
        }

        public async Task Delete(User entity)
        {
            _dbContext.Users.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<User>> GetAll()
        {
            return await _dbContext.Users.Include(u => u.Roles).ToListAsync();
        }

        public User GetById(long id)
        {
            return _dbContext.Users.Include(u => u.Roles)
                             .FirstOrDefault(u => u.Id == id);
        }

        public bool IsUserExists(string email)
        {
            return _dbContext.Users.AsQueryable()
                .Where(x => x.Email.ToLower().Trim() == email.ToLower().Trim())
                .Any();
        }

        public async Task<User> Update(User entity)
        {
            var user = _dbContext.Users.Update(entity);
            await _dbContext.SaveChangesAsync();
            return user.Entity;
        }
    }
}
