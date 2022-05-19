using Conduit.Data;
using Conduit.Data.Repositories;
using Conduit.Domain;
using Microsoft.EntityFrameworkCore;

namespace Tests.Repositories;

public class ArticleRepositoryTest
{
    private readonly DbContextOptionsBuilder<ConduitDbContext> _optionsBuilder;

    public ArticleRepositoryTest()
    {
        _optionsBuilder = new DbContextOptionsBuilder<ConduitDbContext>();
    }
    [Fact]
    public async Task ShouldAddNewArticle()
    {
        _optionsBuilder.UseInMemoryDatabase("AddArticle");
        using (var context = new ConduitDbContext(_optionsBuilder.Options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var articleRepo = new ArticleRepository(context);
            var oldCount = await articleRepo.GetCurrentArticleCount();
            var CreatedArticle = new Articles
            {
                Username = "aya",
                Id = 4,
                Title = "New Article",
                Description = "How to add new object",
                Body = "Use EF core to add object to DB",
                Date = new DateTime(2022,5,19)
            };
            await articleRepo.CreateArticle(CreatedArticle);
            await articleRepo.Save();
            var newCount = await articleRepo.GetCurrentArticleCount();
            Assert.NotEqual(newCount, oldCount);
            Assert.True(newCount > oldCount);
        }
    }

}