using System.CodeDom.Compiler;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Conduit.API.Credentials;
using Conduit.Data.IRepositories;
using Conduit.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Conduit.API.Controllers;

[ApiController]
[Route("api/users")]
public class AuthenticationController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public AuthenticationController(IConfiguration configuration, IUserRepository userRepository,IMapper mapper)
    {
        _configuration = configuration;
        _userRepository = userRepository;
        _mapper = mapper;
    }
    /// <summary>
    /// Login to Conduit
    /// </summary>
    /// <param name="loginCredentials">Provide user's email and password</param>
    /// <returns>JWT</returns>
    /// <response code="200">Returns generated token when the user exists</response>
    /// <response code="404">When a user's email does not exist or the password is wrong</response>

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCredentials loginCredentials)
    {
        var user = await _userRepository.CheckUser(loginCredentials.Email, loginCredentials.Password);
        if (user != null)
        {
            var token = GenerateToken(user);
            return Ok(token);
        }
        return NotFound("User not found");
    }

    /// <summary>
    /// Register new user.
    /// </summary>
    /// <param name="RegistrationCredentials">Provide user's email, username and password</param>
    /// <returns>JWT</returns>
    /// <response code="200">Returns generated token when the user created</response>
    /// <response code="400">When the entered email or username exists</response>
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Register(RegistrationCredentials registrationCredentials)
    {
        var user = _mapper.Map<Users>(registrationCredentials);
        var creatingUser = await _userRepository.CreateUser(user);
        if (creatingUser.isValid)
        {
            var token = GenerateToken(user);
            await _userRepository.Save();
            return Ok(token);
        }
        return BadRequest(creatingUser.message);
    }
    
    [NonAction]
    private string GenerateToken(Users user)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credential = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Username),
            new Claim(ClaimTypes.Email, user.Email)
        };
        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(20),
            signingCredentials:credential
        );
        return new JwtSecurityTokenHandler().WriteToken(token); 
    }
}
