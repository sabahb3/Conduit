using Conduit.API.Credentials;
using Conduit.Data.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.API.Controllers;

[ApiController]
[Route("api/users")]
public class AuthenticationController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public AuthenticationController(IConfiguration configuration, IUserRepository userRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCredentials loginCredentials)
    {
        var user = await _userRepository.CheckUser(loginCredentials.Email, loginCredentials.Password);
        if (user != null)
        {
            return Ok();
        }
        return NotFound("User not found");
    }
}