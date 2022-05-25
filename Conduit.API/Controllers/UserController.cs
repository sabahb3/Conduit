using System.Security.Claims;
using AutoMapper;
using Conduit.API.Validators;
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

    public UserController(IUserRepository userRepository,IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrentUser()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity == null) return null;
        var userName = identity.Claims.FirstOrDefault(o=>o.Type==ClaimTypes.NameIdentifier)?.Value;
        var user = await _userRepository.GetUser(userName!);
        if (user == null) return Unauthorized();
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
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity == null) return Unauthorized();
        var userName = identity.Claims.FirstOrDefault(o=>o.Type==ClaimTypes.NameIdentifier)?.Value;
        var user = await _userRepository.GetUser(userName!);
        if (user == null) return NotFound();
        var userToUpdate = _mapper.Map<UserForUpdatingDto>(user);
        patchDocument.ApplyTo(userToUpdate);
        if (!TryValidateModel((userToUpdate)))
        {
            return ValidationProblem(ModelState);
        }
        _mapper.Map(userToUpdate, user);
        await _userRepository.UpdateUser(user);
        await _userRepository.Save();
        return Ok(user);
    }
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody]UserForUpdatingDto userForUpdatingDto)
    {
        var validator = new UserEditingValidator();  
        var validRes = validator.Validate(userForUpdatingDto); 
        if (!validRes.IsValid) return ValidationProblem(ModelState);
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity == null) return Unauthorized();
        var userName = identity.Claims.FirstOrDefault(o=>o.Type==ClaimTypes.NameIdentifier)?.Value;
        var user = await _userRepository.GetUser(userName!);
        if (user == null) return NotFound();
        _mapper.Map(userForUpdatingDto, user);
        await _userRepository.UpdateUser(user);
        await _userRepository.Save();
        Console.WriteLine(user);
        return Ok(user);
    }
    public override ActionResult ValidationProblem([ActionResultObjectValue]ModelStateDictionary modelStateDictionary)
    {
        Console.WriteLine($"{modelStateDictionary.ValidationState} {modelStateDictionary.IsValid}");
        var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
        return (ActionResult) options.Value.InvalidModelStateResponseFactory(ControllerContext);
    }
}