using Conduit.API.Helper;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Conduit.API.Controllers;
[ApiController]
[Route("api/user/logout")]
public class LogoutController : ControllerBase
{
    private readonly IConnectionMultiplexer _redis;

    public LogoutController(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }
    /// <summary>
    /// Logout current user
    /// </summary>
    /// <returns></returns>
    /// <response code="401">Unauthorized user try to logout</response>
    /// <response code="204">Successful logout</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout()
    {
        var tokenString = await HttpContext.GetTokenAsync("access_token");
        if (tokenString == null) return Unauthorized();
        var token = new JwtSecurityToken(tokenString);
        var expired = token.ValidTo - DateTime.UtcNow;
        var username = token.Claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)!.Value;
        var expiredString =  token.Claims.FirstOrDefault(o => o.Type == "exp")!.Value;
        if (expired.TotalSeconds>0)
            await _redis.GetDatabase().StringSetAsync(username+expiredString,tokenString,expired,When.NotExists,CommandFlags.None);
        return NoContent();
    }
}