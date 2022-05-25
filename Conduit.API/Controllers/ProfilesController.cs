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
    [HttpGet("{username}",Name = "GetUserProfile")]
    public async Task<IActionResult> GetProfile(string username)
    {
        var profile =await PrepareProfile(username);
        return Ok(profile);
    }

    [HttpPost("{followerName}/follow")]
    public async Task<IActionResult> FollowUser(string followerName)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity == null) return Unauthorized();
        var username = identity.Claims.FirstOrDefault(o=>o.Type==ClaimTypes.NameIdentifier)?.Value;
        var user = await _userRepository.GetUser(username!);
        if (user == null)
            return NotFound();
        var following = await _userRepository.GetUser(followerName);
        if (following == null)
            return NotFound();
        await _userRepository.FollowUser(username!, followerName);
        await _userRepository.Save();
        var profile = await PrepareProfile(followerName);
        return Ok(profile!);
    }

    [HttpDelete("{followerName}/follow")]
    public async Task<ActionResult<ProfileDto>> UnfollowUser(string followerName)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity == null) return Unauthorized();
        var username = identity.Claims.FirstOrDefault(o=>o.Type==ClaimTypes.NameIdentifier)?.Value;
        var user = await _userRepository.GetUser(username!);
        if (user == null)
            return NotFound();
        var following = await _userRepository.GetUser(followerName);
        if (following == null)
            return NotFound();
        await _userRepository.UnfollowUser(username!, followerName);
        await _userRepository.Save();
        var profile = await PrepareProfile(followerName);
        return Ok(profile!); 
    }

    [NonAction]
    private async Task<ProfileDto?> PrepareProfile(string username)
    {
        var userEntity = await _userRepository.GetUser(username);
        if (userEntity == null)
            return null;
        var profile=_mapper.Map<ProfileDto>(userEntity);
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity != null)
        {
            var userIdentity = identity.Claims.FirstOrDefault(o=>o.Type==ClaimTypes.NameIdentifier)?.Value;
            profile.Following= await _userRepository.DoesFollow(userIdentity!,username);
        }
        return profile;
    }
}