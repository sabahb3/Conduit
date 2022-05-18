using Conduit.Data.IRepositories;
using Conduit.Data.Models;
using  Conduit.Domain;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Data.Repositories;

public class UserRepository : IUserRepository
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

    public async Task<(bool isValid,List<string> message)> CreateUser(Users createdUser)
    {
        bool valid=true;
        var message = new List<string>();
        if (await IsExists(createdUser.Username))
        {
            valid = false;
            message.Add("username has already been taken");
        }

        if (!await IsUniqueEmail(createdUser.Email))
        {
            valid = false;
            message.Add("email has already been taken");
        }
        if (valid)
        {
            await _conduitDbContext.Users.AddAsync(createdUser);
        }
        return (valid, message);
    }

    public async Task<bool> IsExists(string username)
    {
        var user =await _conduitDbContext.Users.FindAsync(username);
        if (user == null)
            return false;
        return true;
    }

    public async Task<bool> IsUniqueEmail(string email)
    {
        var existingEmails = _conduitDbContext.Users.Select(u => u.Email).FirstOrDefault(u => u == email);
        if (existingEmails == null)
            return true;
        return false;
    }

    public async Task<int> Save()
    {
        var affected=await _conduitDbContext.SaveChangesAsync();
        return affected;
    }
}