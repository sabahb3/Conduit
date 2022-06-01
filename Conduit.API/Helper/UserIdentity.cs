using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using AutoMapper;
using Conduit.Data.IRepositories;
using Conduit.Data.Models;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Conduit.API.Helper;

public class UserIdentity : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IConnectionMultiplexer _redis;

    public UserIdentity(IUserRepository userRepository,IMapper mapper,IConnectionMultiplexer redis)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _redis = redis;
    }

    public string? GetLoggedUser(IIdentity? userIdentity)
    {
        var identity = userIdentity as ClaimsIdentity;
        return identity == null ? 
            null : identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
    }
    public async Task<string?> GetLoggedUser(IIdentity? userIdentity,string? token)
    {
        if (userIdentity is not ClaimsIdentity identity) return null;
        var isLogout = await CheckLogout(token);
        return isLogout is null or true ? null : identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
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
    public async Task<bool>IsExisted(string username)
    {
        username = username.Trim();
        var user = await _userRepository.GetUser(username);
        return user != null;
    }

    public async Task<bool?> CheckLogout(string? tokenString)
    {
        if (tokenString == null) return null;
        var token = new JwtSecurityToken(tokenString);
        var username = token.Claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)!.Value;
        var expired =  token.Claims.FirstOrDefault(o => o.Type == "exp")!.Value;
        var isLogout =await _redis.GetDatabase().StringGetAsync(username + expired);
        if (isLogout == RedisValue.Null) return false;
        return true;
    }
}