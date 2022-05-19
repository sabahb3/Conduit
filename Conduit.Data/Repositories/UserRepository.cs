using Conduit.Data.IRepositories;
using Conduit.Data.Models;
using  Conduit.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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

    public async Task<(bool isValid,List<string> message,EntityEntry<Users>? userEntity)> CreateUser(Users createdUser)
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
        EntityEntry<Users>? userEntity=null;
        if (valid)
        {
            userEntity=await _conduitDbContext.Users.AddAsync(createdUser);
        }
        return (valid, message,userEntity);
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

    public async Task CreateUser(List<Users> createdUser)
    {
        var usersEntity = new List<EntityEntry<Users>>();
        foreach (var user in createdUser)
        {
            var result=await CreateUser(user);
            if(result.isValid)
                usersEntity.Add(result.userEntity!);
            else
            {
                UndoUserCreation(usersEntity);
                break;
            }
        }
    }
    public async Task UndoUserCreation(List<EntityEntry<Users>> users)
    {
        foreach (var user in users)
        {
            user.State = EntityState.Detached;
        }
    }

    public async Task<Users?> GetUser(string username)
    {
        return await _conduitDbContext.Users.FindAsync(username);
    }

    public async Task<IEnumerable<Users>> GetAllUsers()
    {
        return await _conduitDbContext.Users.ToListAsync();
    }

    public async Task UpdateUser(string username, Users updatedUser)
    {
        throw new NotImplementedException();
    }
}