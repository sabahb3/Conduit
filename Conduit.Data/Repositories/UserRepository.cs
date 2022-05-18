using Conduit.Data.Models;
using  Conduit.Domain;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Data.Repositories;

public class UserRepository
{
    private readonly ConduitDbContext _conduitDbContext;

    public UserRepository(ConduitDbContext conduitDbContext)
    {
        _conduitDbContext = conduitDbContext;
    }

    public async Task<int> GetCurrentUsersCount()
    {
        var currentUser= await _conduitDbContext.Users.CountAsync();
        return currentUser;
    }

    public async Task CreateUser(Users createdUser)
    {
        var user = await _conduitDbContext.Users.AddAsync(createdUser);
    }

    public async Task<int> Save()
    {
        var affected=await _conduitDbContext.SaveChangesAsync();
        return affected;
    }
}