using AutoMapper;
using task.Dtos;
using task.Models;
using task.Repositories.IRepositories;
using task.Services.IServices;

namespace task.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        public RoleService(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<Role> Create(Role entity)
        {
            var result = _roleRepository.IsRoleExists(entity.Status);
            if (result)
            {
                throw new ArgumentException("Role is already exists");
            }
            return await _roleRepository.Create(entity);
        }

        public async Task Delete(long id)
        {
            Role role = await _roleRepository.GetById(id);
            if (role == null)
            {
                throw new ArgumentException("There are such no role in database");
            }
            await _roleRepository.Delete(role);
        }

        public async Task<List<Role>> GetAll()
        {
            return await _roleRepository.GetAll();
        }

        public async Task<Role> GetById(long id)
        {
            Role role = await _roleRepository.GetById(id);
            if (role == null)
            {
                throw new ArgumentException("There are such no role in database");
            }
            return role;
        }

        public async Task<Role> Update(Role entity)
        {
            return await _roleRepository.Update(entity);
        }
    }
}
