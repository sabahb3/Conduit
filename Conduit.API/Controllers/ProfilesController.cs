using Conduit.API.Helper;
using Conduit.Data.IRepositories;
using Conduit.Data.Models;
using Microsoft.AspNetCore.Authentication;
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

    /// <summary>
    /// Follow a user
    /// </summary>
    /// <param name="followerName">username of the user you want to follow</param>
    /// <returns>Follower's profile</returns>
    /// <response code="200">Returns Follower's profile</response>
    /// <response code="404">No user with the provided username</response>
    /// <response code="401">Unauthorized user</response>
    [HttpPost("{followerName}/follow")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> FollowUser(string followerName)
    {
        var username = await _userIdentity.GetLoggedUser(HttpContext.User.Identity,await HttpContext.GetTokenAsync("access_token"));
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
    /// <summary>
    /// Unfollow a user
    /// </summary>
    /// <param name="followerName">username of the user you want to unfollow</param>
    /// <returns>User's profile</returns>
    /// <response code="200">Returns user's profile</response>
    /// <response code="404">No user with the provided username</response>
    /// <response code="401">Unauthorized user</response>
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpDelete("{followerName}/follow")]
    public async Task<ActionResult<ProfileDto>> UnfollowUser(string followerName)
    {
        var username = await _userIdentity.GetLoggedUser(HttpContext.User.Identity,await HttpContext.GetTokenAsync("access_token"));
        if (username == null) return Unauthorized();
        var user = await _userRepository.GetUser(username!);
        if (user == null) return NotFound();
        var following = await _userRepository.GetUser(followerName);
        if (following == null) return NotFound();
        await _userRepository.UnfollowUser(username!, followerName);
        await _userRepository.Save();
        var profile = await _userIdentity.PrepareProfile(HttpContext.User.Identity,followerName);
        return Ok(profile!); 
    }
}