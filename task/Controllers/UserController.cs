using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using task.Dtos;
using task.Exceptions;
using task.Models;
using task.Services.IServices;

namespace task.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        ///  Get all users with roles or sorting users by criteria.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="criteria"></param>
        [Authorize]
        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll(int? page, [FromQuery] CriteriaDto criteria)
        {
            return Ok(_mapper.Map<List<UserDto>>(await _userService.SortByCriteria(page, _mapper.Map<Criteria>(criteria))));
        }

        /// <summary>
        ///  Get all users with roles or filtering users by criteria.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="criteria"></param>
        [Authorize]
        [HttpGet]
        [Route("filter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDto>>> FilterByCriteria(int? page, [FromQuery] CriteriaDto criteria)
        {
            return Ok(_mapper.Map<List<UserDto>>(await _userService.FilterByCriteria(page, _mapper.Map<Criteria>(criteria))));
        }

        /// <summary>
        ///  Get user with specific id.
        /// </summary>
        /// <param name="id"></param>
        [Authorize]
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> GetById(long id)
        {
            return Ok(_mapper.Map<UserDto>(await _userService.GetById(id)));
        }

        /// <summary>
        ///  Add specific role to user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        [Authorize]
        [HttpPost]
        [Route("{userId}/roles/{roleId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> AddRole(long userId, long roleId)
        {
            return Ok(_mapper.Map<UserDto>(await _userService.AddRole(userId, roleId)));
        }

        /// <summary>
        ///  Remove specific role from user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        [Authorize]
        [HttpDelete]
        [Route("{userId}/roles/{roleId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> RemoveRole(long userId, long roleId)
        {
            return Ok(_mapper.Map<UserDto>(await _userService.RemoveRole(userId, roleId)));
        }

        /// <summary>
        ///  Add new user.
        /// </summary>
        /// <param name="createUserDto"></param>
        [Authorize]
        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<CreateUserDto>> Create([FromBody] CreateUserDto createUserDto)
        {
            var userAdded = _mapper.Map<UserDto>(await _userService.Create(_mapper.Map<CreateUser>(createUserDto)));
            return CreatedAtAction("GetById", new { id = userAdded.Id }, userAdded);
        }

        /// <summary>
        ///  Update user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userDto"></param>
        [Authorize]
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> Update(long id, [FromBody] UpdateUserDto userDto)
        {
            return Ok(_mapper.Map<UserDto>(await _userService.Update(_mapper.Map<User>(userDto))));
        }

        /// <summary>
        ///  Delete user by id.
        /// </summary>
        /// <param name="id"></param>
        [Authorize]
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteById(long id)
        {
            await _userService.Delete(id);
            return NoContent();
        }

        /// <summary>
        ///  Register new user.
        /// </summary>
        /// <param name="createUserDto"></param>
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<CreateUserDto>> Registration([FromBody] CreateUserDto createUserDto)
        {
            var userAdded = _mapper.Map<UserDto>(await _userService.Create(_mapper.Map<CreateUser>(createUserDto)));
            return CreatedAtAction("GetById", new { id = userAdded.Id }, userAdded);
        }

        /// <summary>
        ///  Login.
        /// </summary>
        /// <param name="authDto"></param>
        [HttpPost]
        [Route("LogIn")]
        public async Task<ActionResult<string>> Login([FromBody] AuthDto authDto)
        {
            return Ok(await _userService.Login(_mapper.Map<Auth>(authDto)));
        }
    }
}
