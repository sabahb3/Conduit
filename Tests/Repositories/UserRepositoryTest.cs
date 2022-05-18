using Conduit.Data;
using Conduit.Data.Repositories;
using Conduit.Domain;
using Microsoft.EntityFrameworkCore;

namespace Tests.Repositories;

public class UserRepositoryTest
{
    private readonly DbContextOptionsBuilder<ConduitDbContext> _optionsBuilder;

    public UserRepositoryTest()
    {
        _optionsBuilder = new DbContextOptionsBuilder<ConduitDbContext>();
    }

    [Fact]
    public async Task ShouldAddNewUser()
    {
        _optionsBuilder.UseInMemoryDatabase("AddUser");
        using (var context = new ConduitDbContext(_optionsBuilder.Options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var userRepo = new UserRepository(context);
            var oldCount = await userRepo.GetCurrentUsersCount();
            var CreatedUser = new Users
            {
                Username = "aya",
                Password = 1234.ToString(),
                Email = "ayaJamal@gmail.com",
                ProfilePicture = "test photo"
            };
            await userRepo.CreateUser(CreatedUser);
            await userRepo.Save();
            var newCount = await userRepo.GetCurrentUsersCount();
            Assert.NotEqual(newCount, oldCount);
            Assert.True(newCount > oldCount);
        }
    }

    [Fact]
    public async Task ShouldNotAddNewUser()
    {
        _optionsBuilder.UseInMemoryDatabase("AddUser");
        using (var context = new ConduitDbContext(_optionsBuilder.Options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var userRepo = new UserRepository(context);
            var oldCount = await userRepo.GetCurrentUsersCount();
            var CreatedUser = new Users
            {
                Username = "sabah",
                Password = 1234.ToString(),
                Email = "sabahbaara@gmail.com",
                ProfilePicture = "test photo"
            };
            await userRepo.CreateUser(CreatedUser);
            await userRepo.Save();
            var newCount = await userRepo.GetCurrentUsersCount();
            Assert.Equal(newCount, oldCount);
        }
    }
}