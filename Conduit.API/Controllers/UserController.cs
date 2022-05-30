using AutoMapper;
using Conduit.API.Helper;
using Conduit.Data.IRepositories;
using Conduit.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace Conduit.API.Controllers;

[ApiController]
[Route("api/user")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly UserIdentity _userIdentity;

    public UserController(IUserRepository userRepository,IMapper mapper,UserIdentity userIdentity)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _userIdentity = userIdentity;
    }
    /// <summary>
    /// Get current user
    /// </summary>
    /// <returns>Return current user which consists of a username, email, bio, image, and token</returns>
    /// <response code="200">When the token is valid it returns a user</response>
    /// <response code="401">When the token is invalid or there is no token</response>
    /// <response code="404">When the token is valid but the user does not exist anymore</response>

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userName = _userIdentity.GetLoggedUser(HttpContext.User.Identity);
        if (userName == null) return Unauthorized();
        var user = await _userRepository.GetUser(userName!);
        if (user == null) return NotFound();
        var userToReturn = _mapper.Map<UserForReturningDto>(user!);
        var token = await HttpContext.GetTokenAsync("access_token");
        userToReturn.Token = token!;
        return Ok(userToReturn);
    }
        /// <summary>
    /// Partially update user
    /// </summary>
    /// <param name="patchDocument">The set of operations to apply on the user</param>
    /// <returns> </returns>
    /// <remarks>
    /// Sample Request : This will update user username  
    ///```  
    /// [  
    /// {  
    ///    "op": "replace",  
    ///    "path": "/username",  
    ///    "value": "sab"  
    /// }  
    /// ]  
    ///```  
    /// </remarks>>
    [HttpPatch]
    public async Task<IActionResult> PartialUpdateUser(JsonPatchDocument<UserForUpdatingDto>patchDocument)
    {
        var userName = _userIdentity.GetLoggedUser(HttpContext.User.Identity);
        if (userName == null) return Unauthorized();
        var user = await _userRepository.GetUser(userName!);
        if (user == null) return NotFound();
        var userToUpdate = _mapper.Map<UserForUpdatingDto>(user);
        patchDocument.ApplyTo(userToUpdate);
        if (!TryValidateModel((userToUpdate)))
        {
            return ValidationProblem(ModelState);
        }
        _mapper.Map(userToUpdate, user);
        await _userRepository.UpdateUser(user,userName!);
        var affected= await _userRepository.Save();
        var token = await HttpContext.GetTokenAsync("access_token");
        if (affected == 0)
        {
            var unUpdatedUser =await _userRepository.GetUser(userName!);
            var unUpdatedDto = _mapper.Map<UserForReturningDto>(unUpdatedUser);
            unUpdatedDto.Token = token!;
            return Ok( unUpdatedDto);
        }
        var userDto = _mapper.Map<UserForReturningDto>(user);
        userDto.Token = token!;
        return Ok(userDto);
    }
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody]UserForUpdatingDto userForUpdatingDto)
    {
        var userName = _userIdentity.GetLoggedUser(HttpContext.User.Identity);
        if (userName == null) return Unauthorized();
        if(!TryValidateModel(userForUpdatingDto)) return ValidationProblem(ModelState);
        var user = await _userRepository.GetUser(userName!);
        if (user == null) return NotFound();
        _mapper.Map(userForUpdatingDto, user);
        await _userRepository.UpdateUser(user,userName!);
       var affected= await _userRepository.Save();
       var token = await HttpContext.GetTokenAsync("access_token");
       if (affected == 0)
       {
           var unUpdatedUser =await _userRepository.GetUser(userName!);
           var unUpdatedDto = _mapper.Map<UserForReturningDto>(unUpdatedUser);
           unUpdatedDto.Token = token!;
           return Ok( unUpdatedDto);
       }
       var userDto = _mapper.Map<UserForReturningDto>(user);
       userDto.Token = token!;
       return Ok(userDto);
    }
    public override ActionResult ValidationProblem([ActionResultObjectValue]ModelStateDictionary modelStateDictionary)
    {
        Console.WriteLine($"{modelStateDictionary.ValidationState} {modelStateDictionary.IsValid}");
        var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
        return (ActionResult) options.Value.InvalidModelStateResponseFactory(ControllerContext);
    }
}