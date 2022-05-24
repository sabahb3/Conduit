using System.Security.Claims;
using AutoMapper;
using Conduit.Data.IRepositories;
using Conduit.Data.Models;
using Conduit.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.API.Controllers;

[ApiController]
[Route("api/user")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserController(IUserRepository userRepository,IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrentUser()
    {
        var user = await GetAuthenticatedUser();
        if (user == null) return Unauthorized();
        var userToReturn = _mapper.Map<UserForReturningDto>(user!);
        var token = await HttpContext.GetTokenAsync("access_token");
        userToReturn.Token = token!;
        return Ok(userToReturn);
    }
    
    public async Task<Users?> GetAuthenticatedUser()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity == null) return null;
        var userName = identity.Claims.FirstOrDefault(o=>o.Type==ClaimTypes.NameIdentifier)?.Value;
        return await _userRepository.GetUser(userName!);
    }
}