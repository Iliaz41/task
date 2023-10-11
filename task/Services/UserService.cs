using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Nodes;
using task.Dtos;
using task.Exceptions;
using task.Models;
using task.Repositories;
using task.Repositories.IRepositories;
using task.Services.IServices;

namespace task.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleService _roleService;
        private readonly ILogger<UserService> _logger;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IRoleService roleService, ILogger<UserService> logger, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _roleService = roleService;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<string> Login(Auth entity)
        {
            var users = await _userRepository.GetAll();
            var user = new User();
            foreach(var i in users)
            {
                if (entity.Email == i.Email)
                {
                    user = i;
                }

            }
            if (user == null)
            {
                throw new BadRequestException("User not found.");
            }
            if (!BCrypt.Net.BCrypt.Verify(entity.Password, user.Password))
            {
                throw new BadRequestException("Wrong password.");
            }
            string token = await CreateToken(entity);
            var response = new JsonObject();
            response.Add("accessToken", token);
            response.Add("expTime", DateTime.Now);
            response.Add("userId", user.Id);
            return response.ToString();
        }

        public async Task<string> CreateToken(Auth entity)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Email, entity.Email)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public async Task<User> AddRole(long userId, long roleId)
        {
            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                _logger.LogError($"There is no such in database user with this id");
                throw new NotFoundException("There is no such user in database");
            }
            user.Roles.Add(await _roleService.GetById(roleId));
            return await _userRepository.Update(user);
        }

        public async Task<User> Create(CreateUser entity)
        {
            var result = _userRepository.IsUserExists(entity.Email);
            if (result)
            {
                _logger.LogError($"User already exists");
                throw new BadRequestException("User already exists");
            }
            var user = new User
            {
                Name = entity.Name,
                Age = entity.Age,
                Email = entity.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(entity.Password)
        };
            var role = await _roleService.GetById(entity.RoleId);
            if (role == null)
            {
                _logger.LogError($"There is no such role in database");
                throw new NotFoundException("There is no such role in database");
            }
            else
            {
                user.Roles.Add(role);
            }
            return await _userRepository.Create(user);
        }

        public async Task Delete(long id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
            {
                _logger.LogError($"There is no such in database user with this id");
                throw new NotFoundException("There is no such user in database");
            }
            await _userRepository.Delete(user);
        }

        public async Task<User> GetById(long id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
            {
                _logger.LogError($"There is no such user with this id in database");
                throw new NotFoundException("There is no such user in database");
            }
            return user;
        }

        public async Task<User> Update(User entity)
        {
            return await _userRepository.Update(entity);
        }

        public async Task<User> RemoveRole(long userId, long roleId)
        {
            var role = await _roleService.GetById(roleId);
            if (role == null)
            {
                _logger.LogError($"There is no such in database role with this id");
                throw new NotFoundException("There is no such role in database");
            }
            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                _logger.LogError($"There is no such in database user with this id");
                throw new NotFoundException("There is no such user in database");
            }
            user.Roles.Remove(role);
            return await _userRepository.Update(user);
        }

        public async Task<List<User>> SortByCriteria(int? page, Criteria criteria)
        {
            const int pageSize = 5;
            var users = await _userRepository.GetAll();
            if (users == null)
            {
                _logger.LogError($"There are no such users in database");
                throw new NotFoundException("There are no such users in database");
            }
            if (criteria.IsDesc == true)
            {
                if (criteria.Age != null) 
                {
                    users = (from user in users
                             orderby user.Age descending
                             select user).ToList();
                }
                else if (criteria.Name != null)
                {
                    users = (from user in users
                             orderby user.Name descending
                             select user).ToList();
                }
                else if (criteria.Email != null)
                {
                    users = (from user in users
                             orderby user.Email descending
                             select user).ToList();
                }
                else if (criteria.Status != null)
                {
                    users = (from user in users
                             from role in user.Roles
                             orderby role.Status descending
                             select user).ToList();
                }
                else
                {
                    users = (from user in users
                             orderby user.Id descending
                             select user).ToList();
                }
                return users = users.Skip((page ?? 0) * pageSize)
                                          .Take(pageSize)
                                          .ToList();
            }
            else
            {
                if (criteria.Age != null)
                {
                    users = (from user in users
                             orderby user.Age
                             select user).ToList();
                }
                else if (criteria.Name != null)
                {
                    users = (from user in users
                             orderby user.Name
                             select user).ToList();
                }
                else if (criteria.Email != null)
                {
                    users = (from user in users
                             orderby user.Email
                             select user).ToList();
                }
                else if (criteria.Status != null) 
                {
                    users = (from user in users
                             from role in user.Roles
                             orderby role.Status
                             select user).ToList();
                }
                return users = users.Skip((page ?? 0) * pageSize)
                                          .Take(pageSize)
                                          .ToList();
            }
        }

        public async Task<List<User>> FilterByCriteria(int? page, Criteria criteria)
        {
            const int pageSize = 5;
            var users = await _userRepository.GetAll();
            if (users == null)
            {
                _logger.LogError($"There are no such users in database");
                throw new NotFoundException("There are no such users in database");
            }
            if (criteria.Age != null)
            {
                users = (from user in users
                        where user.Age == criteria.Age
                        select user).ToList();
            }
            if (criteria.Name != null)
            {
                users = (from user in users
                         where user.Name == criteria.Name
                         select user).ToList();
            }
            if (criteria.Email != null)
            {
                users = (from user in users
                                where user.Email == criteria.Email
                                select user).ToList();
            }
            if (criteria.Status != null)
            {
                users = (from user in users
                                from role in user.Roles
                                where role.Status == criteria.Status
                                select user).ToList();
            }
            return users = users.Skip((page ?? 0) * pageSize)
                                          .Take(pageSize)
                                          .ToList();
        }
    }
}
