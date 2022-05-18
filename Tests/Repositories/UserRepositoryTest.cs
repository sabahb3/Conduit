using Conduit.Data;
using Conduit.Data.Repositories;
using Conduit.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Tests.Repositories;

public class UserRepositoryTest
{
    private DbContextOptionsBuilder<ConduitDbContext> _optionsBuilder;
    public UserRepositoryTest()
    {
        _optionsBuilder = new DbContextOptionsBuilder<ConduitDbContext>();

    }
    [Fact]
    public async Task ShouldAddNewUser()
    {
        _optionsBuilder.UseInMemoryDatabase("AddUser");
        using (var context= new ConduitDbContext(_optionsBuilder.Options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var userRepo = new UserRepository(context);
            var oldCount = await userRepo.GetCurrentUsersCount();
            var CreatedUser = new UserForCreationDto
            {
                Username = "aya",
                Password = 1234.ToString(),
                Email = "ayaJamal@gmail.com"
            };
            var newAuthor = await userRepo.CreateUser(CreatedUser);
            var newCount= await userRepo.GetCurrentUsersCount();
            Assert.NotEqual(newCount,oldCount);
            Assert.True(newCount>oldCount);
        }
    }
}