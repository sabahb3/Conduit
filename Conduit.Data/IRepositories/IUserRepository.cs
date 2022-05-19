using Conduit.Domain;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Conduit.Data.IRepositories;

public interface IUserRepository
{
    public Task<int> GetCurrentUsersCount();
    public Task<(bool isValid,List<string> message,EntityEntry<Users>? userEntity)> CreateUser(Users createdUser);
    public Task CreateUser(List<Users> createdUser);
    public Task<int> Save();
    public Task<bool> IsExists(string username);


}