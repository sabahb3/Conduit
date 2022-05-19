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
    public Task<Users?> GetUser(string username);
    public Task<IEnumerable<Users>> GetAllUsers();
    public Task<Users?> UpdateUser(Users updatedUser);
    public Task UpdateUsers(List<Users> updatedUsers);


}