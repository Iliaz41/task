using Microsoft.EntityFrameworkCore;
using task.Data;
using task.Models;
using task.Repositories.IRepositories;

namespace task.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public RoleRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Role> Create(Role entity)
        {
            var role = await _dbContext.Roles.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return role.Entity;
        }

        public async Task Delete(Role entity)
        {
            _dbContext.Roles.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Role>> GetAll()
        {
            return await _dbContext.Roles.ToListAsync();
        }

        public async Task<Role> GetById(long id)
        {
            return await _dbContext.Roles.FindAsync(id);
        }

        public bool IsRoleExists(string status)
        {
            return _dbContext.Roles.AsQueryable()
                .Where(x => x.Status.ToLower().Trim() == status.ToLower().Trim())
                .Any();
        }

        public async Task<Role> Update(Role entity)
        {
            var role = _dbContext.Roles.Update(entity);
            await _dbContext.SaveChangesAsync();
            return role.Entity;
        }
    }
}
