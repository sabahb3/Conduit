using System.Security.Claims;
using AutoMapper;
using Conduit.Data.IRepositories;
using Conduit.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.API.Controllers;

[ApiController]
[Route("api/profiles")]
public class ProfilesController :ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public ProfilesController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    [AllowAnonymous]
    [HttpGet("{username}")]
    public async Task<IActionResult> GetProfile(string username)
    {
        var userEntity = await _userRepository.GetUser(username);
        if (userEntity == null)
            return NotFound();
        var profile=_mapper.Map<ProfileDto>(userEntity);
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity != null)
        {
            var userIdentity = identity.Claims.FirstOrDefault(o=>o.Type==ClaimTypes.NameIdentifier)?.Value;
            profile.Following= await _userRepository.DoesFollow(userIdentity!,username);
        }
        return Ok(profile);
    }
}