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
                Username = "sabah",
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
    [Fact]
    public async Task ShouldAddUsersList()
    {
        _optionsBuilder.UseInMemoryDatabase("AddArticleList");
        using (var context = new ConduitDbContext(_optionsBuilder.Options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var articleRepo = new ArticleRepository(context);
            var oldCount = await articleRepo.GetCurrentArticleCount();
            var CreatedArticle = new List<Articles>
            {
                new Articles
                {
                    Username = "sabah",
                    Id = 4,
                    Title = "New Article",
                    Description = "How to add new object",
                    Body = "Use EF core to add object to DB",
                    Date = new DateTime(2022,5,19)
                },
                new Articles
                {
                    Username = "Hala",
                    Id = 5,
                    Title = "New Article",
                    Description = "How to add new object",
                    Body = "Use EF core to add object to DB",
                    Date = new DateTime(2022,5,16)
                }
            };
            await articleRepo.CreateArticles(CreatedArticle);
            var affected = await articleRepo.Save();
            var newCount = await articleRepo.GetCurrentArticleCount();
            Assert.NotEqual(newCount, oldCount);
            Assert.True(newCount > oldCount);
            Assert.Equal(CreatedArticle.Count, affected);
        }
    }
    [Fact]
    public async Task ShouldGetArticleInfo()
    {
        _optionsBuilder.UseInMemoryDatabase("GetArticle");
        using (var context = new ConduitDbContext(_optionsBuilder.Options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var articleRepo = new ArticleRepository(context);
            var article = await articleRepo.GetArticle(1);
            Assert.Equal("Hello, and welcome", article.Title);
            Assert.Equal("How to say hello", article.Description);
            Assert.Equal(@"We often start an English conversation with a simple “hello.”
                         You may see someone you know, make eye contact with a stranger, 
                         or start a phone conversation with this simple greeting. 
                         You may be asking yourself: “What should I say instead of “hello?", article.Body);
            Assert.Equal(new DateTime(2022,4,20),article.Date);
            Assert.Equal("Hala", article.Username);
        }
    }
    [Fact]
    public async Task ShouldGetAllArticles()
    {
        _optionsBuilder.UseInMemoryDatabase("GetUArticles");
        using (var context = new ConduitDbContext(_optionsBuilder.Options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var articleRepo = new ArticleRepository(context);
            var articles = await articleRepo.GetAllArticles();
            Assert.Equal(3, articles.Count());
        }
    }
}