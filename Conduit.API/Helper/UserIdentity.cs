using System.Security.Claims;
using System.Security.Principal;
using AutoMapper;
using Conduit.Data.IRepositories;
using Conduit.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.API.Helper;

public class UserIdentity : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserIdentity(IUserRepository userRepository,IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public string? GetLoggedUser(IIdentity? userIdentity)
    {
        var identity = userIdentity as ClaimsIdentity;
        return identity == null ? 
            null : identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
    }
    public async Task<ProfileDto?> PrepareProfile(IIdentity? identity,string username)
    {
        var userEntity = await _userRepository.GetUser(username);
        if (userEntity == null)
            return null;
        var profile = _mapper.Map<ProfileDto>(userEntity);
        var userIdentity = GetLoggedUser(identity);
        if (userIdentity!=null)
        {
            profile.Following = await _userRepository.DoesFollow(userIdentity!, username);
        }
        return profile;
    }
}