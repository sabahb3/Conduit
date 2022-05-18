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

    [Theory]
    [InlineData("sabah","sabahbaara@gmail.com")]
    [InlineData("sabahb3","sabahBaara4@gmail.com")]
    [InlineData("sabah","sabahbaara4@gmail.com")]
    public async Task ShouldNotAddNewUser(string username,string email)
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
                Username = username,
                Password = 1234.ToString(),
                Email = email,
                ProfilePicture = "test photo"
            };
            await userRepo.CreateUser(CreatedUser);
            await userRepo.Save();
            var newCount = await userRepo.GetCurrentUsersCount();
            Assert.Equal(newCount, oldCount);
        }
    }
    [Fact]
    public async Task ShouldAddUsersList()
    {
        _optionsBuilder.UseInMemoryDatabase("AddUser");
        using (var context = new ConduitDbContext(_optionsBuilder.Options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var userRepo = new UserRepository(context);
            var oldCount = await userRepo.GetCurrentUsersCount();
            var CreatedUser = new List<Users>
            {
                new Users
                {
                    Username = "aya",
                    Password = 1234.ToString(),
                    Email = "ayaJamal@gmail.com",
                    ProfilePicture = "test photo"
                },
                new Users
                {
                    Username = "Rahaf",
                    Password = 1234.ToString(),
                    Email = "rahaf44@gmail.com",
                    ProfilePicture = "test photo"
                }
            };
            await userRepo.CreateUser(CreatedUser);
            var affected =await userRepo.Save();
            var newCount = await userRepo.GetCurrentUsersCount();
            Assert.NotEqual(newCount, oldCount);
            Assert.True(newCount > oldCount);
            Assert.Equal(CreatedUser.Count,affected);
        }
    }
}