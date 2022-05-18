using Conduit.Domain;
namespace Conduit.Data.IRepositories;

public interface IUserRepository
{
    public Task<int> GetCurrentUsersCount();
    public Task<(bool isValid,List<string> message)> CreateUser(Users createdUser);
    public Task CreateUser(List<Users> createdUser);
    public Task<int> Save();
    public Task<bool> IsExists(string username);


}