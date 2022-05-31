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
    /// <summary>
    /// Get the user's profile
    /// </summary>
    /// <param name="username">username of selected user</param>
    /// <returns>Profile</returns>
    /// <response code="200">Returns user's profile</response>
    /// <response code="404">When there is no user with this username</response>
    [AllowAnonymous]
    [HttpGet("{username}",Name = "GetUserProfile")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfile(string username)
    {
        var user = await _userRepository.GetUser(username);
        if (user == null) return NotFound();
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