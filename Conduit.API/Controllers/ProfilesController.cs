using System.Security.Claims;
using Conduit.API.Helper;
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
    private readonly UserIdentity _userIdentity;

    public ProfilesController(IUserRepository userRepository,UserIdentity userIdentity)
    {
        _userRepository = userRepository;
        _userIdentity = userIdentity;
    }
    [AllowAnonymous]
    [HttpGet("{username}",Name = "GetUserProfile")]
    public async Task<IActionResult> GetProfile(string username)
    {
        var profile =await _userIdentity.PrepareProfile(HttpContext.User.Identity,username);
        return Ok(profile);
    }

    [HttpPost("{followerName}/follow")]
    public async Task<IActionResult> FollowUser(string followerName)
    {
        var username = _userIdentity.GetLoggedUser(HttpContext.User.Identity);
        if (username == null) return Unauthorized();
        var user = await _userRepository.GetUser(username!);
        if (user == null) return NotFound();
        var following = await _userRepository.GetUser(followerName);
        if (following == null) return NotFound();
        await _userRepository.FollowUser(username!, followerName);
        await _userRepository.Save();
        var profile = await _userIdentity.PrepareProfile(HttpContext.User.Identity,followerName);
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
        var profile = await _userIdentity.PrepareProfile(HttpContext.User.Identity,followerName);
        return Ok(profile!); 
    }
}