using Conduit.Data.Models;
using  Conduit.Domain;
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
        throw new NotImplementedException();
    }

    public async Task<Users> CreateUser(UserForCreationDto createdUser)
    {
        throw new NotImplementedException();
    }
}