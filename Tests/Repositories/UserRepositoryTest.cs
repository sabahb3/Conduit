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
    [Fact]
    public async Task ShouldIgnoreUserList()
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
                    Email = "ayabaara@gmail.com",
                    ProfilePicture = "test photo"
                },
                new Users
                {
                    Username = "sabah",
                    Password = 1234.ToString(),
                    Email = "s.12@gmail.com",
                    ProfilePicture = "test photo"
                }
            };
            await userRepo.CreateUser(CreatedUser);
            await userRepo.Save();
            var newCount = await userRepo.GetCurrentUsersCount();
            Assert.Equal(newCount, oldCount);
        }
    }

    [Fact]
    public async Task ShouldGetUserInfo()
    {
        _optionsBuilder.UseInMemoryDatabase("GetUser");
        using (var context = new ConduitDbContext(_optionsBuilder.Options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var userRepo = new UserRepository(context);
            var user= await userRepo.GetUser("sabah");
            Assert.Equal("sabah",user.Username);
            Assert.Equal("4050",user.Password);
            Assert.Equal("sabahBaara4@gmail.com",user.Email);
            Assert.Null(user.Bio);
            Assert.Equal("https://api.realworld.io/images/smiley-cyrus.jpeg",user.ProfilePicture);
        }
    }

    [Fact]
    public async Task ShouldGetAllUsers()
    {
        _optionsBuilder.UseInMemoryDatabase("GetUser");
        using (var context = new ConduitDbContext(_optionsBuilder.Options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var userRepo = new UserRepository(context);
            var users= await userRepo.GetAllUsers();
            Assert.All(users.Select(u=>u.ProfilePicture), 
                p=>Assert.Equal("https://api.realworld.io/images/smiley-cyrus.jpeg",p));
            Assert.All(users.Select(u=>u.Bio), Assert.Null);
            Assert.Equal(3,users.Count());
        }
    }

    [Fact]
    public async Task ShouldUpdateUserInfo()
    {
        _optionsBuilder.UseInMemoryDatabase("GetUser");
        using (var context = new ConduitDbContext(_optionsBuilder.Options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var userRepo = new UserRepository(context);
            var userToUpdate = await userRepo.GetUser("sabah");
            var updatedUser = new Users
            {
                Username = userToUpdate!.Username,
                Email="sabahb399@gmail.com",
                Bio="Hello",
                ProfilePicture = "Updated photo",
                Password = 1212.ToString()
            };
            await userRepo.UpdateUser(userToUpdate!.Username, updatedUser);
            var affectedUser=await userRepo.Save();
            Assert.Equal("sabah",userToUpdate.Username);
            Assert.Equal("sabahb399@gmail.com",userToUpdate.Email);
            Assert.Equal("Hello",userToUpdate.Bio);
            Assert.Equal("Updated photo",userToUpdate.ProfilePicture);
            Assert.Equal("1212",userToUpdate.Password);
            Assert.Equal(1,affectedUser);
        }
    }
}