using AutoMapper;
using EvaluationProjects.Helpers.Auth;
using EvaluationProjects.Interfaces;
using EvaluationProjects.Models.Constans;
using EvaluationProjects.Models.Dtos.Request;
using EvaluationProjects.Models.Dtos.Response;
using EvaluationProjects.Models.Entities;
using EvaluationProjects.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace EvaluationProjects.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly JwtSecurityToken token;
        private readonly IJwt _generateJwt;
        private readonly IMapper _mapper;
        private readonly IHashingPassword _hashPassword;


        public UserController(IJwt generateJwt, IGenericRepository<User> userRepository, IMapper mapper, IHashingPassword hashPassword, IHttpContextAccessor httpContextAccessor)
        {
            _generateJwt = generateJwt;
            _userRepository = userRepository;
            _mapper = mapper;
            _hashPassword = hashPassword;
            token = httpContextAccessor.HttpContext.Items["JwtToken"] as JwtSecurityToken;
        }


        /// <summary>
        /// Required role program
        /// </summary>
        [HttpPost]
        [Authorize(Roles ="program")]
        public ActionResult Create([FromBody] UserRequestDto userDto)
        {
            //if (!VerifyRole.Verify(token, ROLES.PROGRAM.ToLower())) return Unauthorized("You do not have permissions to access this resource");

            if (userDto == null) return BadRequest();

            if (!userDto.Role.ToLower().Equals(ROLES.STUDENT.ToLower()) 
                && !userDto.Role.ToLower().Equals(ROLES.TEACHER.ToLower()) 
                && !userDto.Role.ToLower().Equals(ROLES.PROGRAM.ToLower())
            )
            {
                return BadRequest("Invalid role.");
            }

            try
            {
                var user = _mapper.Map<User>(userDto);
                user.Password = _hashPassword.HashPassword(userDto.Password);
                _userRepository.Add(user);
                return StatusCode((int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto userDto)
        {
            var user = await _userRepository.GetAsync(x => x.Email.Equals(userDto.UserEmail));

            if(user == null) return NotFound("User not found");

            if (!_hashPassword.VerifyPassword(userDto.Password, user.Password)) return BadRequest("Password incorrect");

            var response = _mapper.Map<LoginResponseDto>(user);

            response.Token = _generateJwt.GenerateJWTToken(user);

            return Ok(response);
        }

        /// <summary>
        /// Required role program
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "program")]
        public async Task<IActionResult> GetAllUsers()
        {

            return StatusCode((int)HttpStatusCode.OK, await _userRepository.GetAllAsync());
        }

        /// <summary>
        /// Required role program
        /// </summary>
        [HttpGet("{role}")]
        [Authorize(Roles ="program")]
        public async Task<IActionResult> GetByRole(string role)
        {

            if (role == null) return BadRequest();

            if (role.ToLower().Equals(ROLES.STUDENT) || role.ToLower().Equals(ROLES.TEACHER) || role.ToLower().Equals(ROLES.PROGRAM))
            {
                return StatusCode((int)HttpStatusCode.OK, await _userRepository.GetListAsync(user => user.Role.Equals(role)));

            }
            return BadRequest("Invalid role.");

        }
    }
}
