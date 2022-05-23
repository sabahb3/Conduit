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
    public Task<bool> IsArticlePreferred(string username, int articleId);
    public Task FavoriteArticle(string username, int articleId);
    public Task UnfavoriteArticle(string username, int articleId);
    public Task<bool> DoesFollow(string username, string followingName);
    public Task FollowUser(string username, string followingName);
    public Task UnfollowUser(string username, string followerName);
    public Task<Users?> CheckUser(string email, string password);
}