using Conduit.Domain;
namespace Conduit.Data.IRepositories;

public interface IUserRepository
{
    public Task<int> GetCurrentUsersCount();
    public Task CreateUser(Users createdUser);
    public Task<int> Save();
    public Task<bool> IsExists(string username);


}