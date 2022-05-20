using Conduit.Data;
using Microsoft.EntityFrameworkCore;
using Conduit.Data.Repositories;
using Conduit.Domain;

namespace Tests.Repositories;

public class CommentRepositoryTest
{
    private readonly DbContextOptionsBuilder<ConduitDbContext> _optionsBuilder;

    public CommentRepositoryTest()
    {
        _optionsBuilder = new DbContextOptionsBuilder<ConduitDbContext>();
    }

    [Fact]
    public async Task ShouldAddNewComment()
    {
        _optionsBuilder.UseInMemoryDatabase("AddComment");
        using (var context = new ConduitDbContext(_optionsBuilder.Options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var commentRepo = new CommentRepository(context);
            var oldCount = await commentRepo.GetCurrentCommentsCount();
            var CreatedComment = new Comments
            {
                Username = "sabah",
                ArticlesId = 1,
                Id = 4,
                Body = "new comment",
                Date = new DateTime(2022,5,19)
            };
            await commentRepo.CreateComment(CreatedComment);
            await commentRepo.Save();
            var newCount = await commentRepo.GetCurrentCommentsCount();
            Assert.NotEqual(newCount, oldCount);
            Assert.True(newCount > oldCount);
        }
    }
    [Fact]
    public async Task ShouldAddCommentsList()
    {
        _optionsBuilder.UseInMemoryDatabase("AddCommentList");
        using (var context = new ConduitDbContext(_optionsBuilder.Options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var commentRepo = new CommentRepository(context);
            var oldCount = await commentRepo.GetCurrentCommentsCount();
            var CreatedComments = new List<Comments>
            {
                new Comments
                {
                    Username = "sabah",
                    ArticlesId = 1,
                    Id = 4,
                    Body = "new comment",
                    Date = new DateTime(2022,5,19)
                },
                new Comments
                {
                    Username = "Hala",
                    ArticlesId = 1,
                    Id = 5,
                    Body = "new comment",
                    Date = new DateTime(2022,5,19)
                }
            };
            await commentRepo.CreateComments(CreatedComments);
            var affected = await commentRepo.Save();
            var newCount = await commentRepo.GetCurrentCommentsCount();
            Assert.NotEqual(newCount, oldCount);
            Assert.True(newCount > oldCount);
            Assert.Equal(CreatedComments.Count, affected);
        }
    }

}