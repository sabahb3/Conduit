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
    public async Task ShouldAddArticlesList()
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
            var articles = await articleRepo.GetAllArticles(0,20);
            Assert.Equal(3, articles.Count());
        }
    }
    [Fact]
    public async Task ShouldUpdateArticleInfo()
    {
        _optionsBuilder.UseInMemoryDatabase("updateArticle");
        using (var context = new ConduitDbContext(_optionsBuilder.Options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var articleRepo = new ArticleRepository(context);
            var articleToUpdate = await articleRepo.GetArticle(3);
            var updatedArticle = new Articles
            {
                Username = "Sabah",
                Id = 3,
                Title = "Updated Article",
                Description = "How to update existing object",
                Body = "Use EF core to update object in DB",
                Date = new DateTime(2022,5,1)
            };
            await articleRepo.UpdateArticle(updatedArticle);
            var affectedUser = await articleRepo.Save();
            Assert.Equal(updatedArticle.Username, articleToUpdate!.Username);
            Assert.Equal(updatedArticle.Id, articleToUpdate.Id);
            Assert.Equal(updatedArticle.Title, articleToUpdate.Title);
            Assert.Equal(updatedArticle.Description, articleToUpdate.Description);
            Assert.Equal(updatedArticle.Body, articleToUpdate.Body);
            Assert.Equal(updatedArticle.Date,articleToUpdate.Date);
            Assert.Equal(1, affectedUser);
        }
    }
    [Fact]
    public async Task ShouldUpdateListOfArticle()
    {
        _optionsBuilder.UseInMemoryDatabase("UpdateArticles");
        using (var context = new ConduitDbContext(_optionsBuilder.Options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var articleRepo = new ArticleRepository(context);
            var updatedArticles = new List<Articles>
            {
                new Articles
                {
                    Username = "Hala",
                    Id = 1,
                    Title = "Updated Article",
                    Description = "How to update existing object",
                    Body = "Use EF core to update object in DB",
                    Date = new DateTime(2022,5,1)
                },
                new Articles
                {
                    Username = "Hala",
                    Id = 2,
                    Title = "Updated Article",
                    Description = "How to update existing object",
                    Body = "Use EF core to update object in DB",
                    Date = new DateTime(2022,5,1)
                },
                new Articles
                {
                    Username = "Sabah",
                    Id = 3,
                    Title = "Updated Article",
                    Description = "How to update existing object",
                    Body = "Use EF core to update object in DB",
                    Date = new DateTime(2022,5,1)
                }
            };
            await articleRepo.UpdateArticles(updatedArticles);
            var affectedUser = await articleRepo.Save();
            Assert.Equal(3,affectedUser);
            Assert.All(context.Articles.Select(u=>u.Title),u=>Assert.Equal( "Updated Article",u));
            Assert.All(context.Articles.Select(u=>u.Description),u=>Assert.Equal( "How to update existing object",u));
            Assert.All(context.Articles.Select(u=>u.Body),u=>Assert.Equal( "Use EF core to update object in DB",u));
            Assert.All(context.Articles.Select(u=>u.Date),u=>Assert.Equal( new DateTime(2022,5,1),u));
        }
    }

    [Fact]
    public async void ShouldDeleteArticle()
    {
        _optionsBuilder.UseInMemoryDatabase("RemoveArticle");
        using (var context = new ConduitDbContext(_optionsBuilder.Options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var articleRepo = new ArticleRepository(context);
            var oldCount = await articleRepo.GetCurrentArticleCount();
            await articleRepo.RemoveArticle(1);
            await articleRepo.Save();
            var newCount = await articleRepo.GetCurrentArticleCount();
            Assert.NotEqual(newCount, oldCount);
            Assert.True(newCount < oldCount);
        }
    }
    [Fact]
    public async Task ShouldDeleteAllArticles()
    {
        _optionsBuilder.UseInMemoryDatabase("RemoveAllArticles");
        using (var context = new ConduitDbContext(_optionsBuilder.Options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var articleRepo = new ArticleRepository(context);
            var oldCount = await articleRepo.GetCurrentArticleCount();
            await articleRepo.RemoveArticles();
            await articleRepo.Save();
            Assert.NotEqual(0,oldCount);
            Assert.Empty(context.Articles);
        }
    }
    [Fact]
    public async Task ShouldCountWhoFavoriteArticle()
    {
        _optionsBuilder.UseInMemoryDatabase("CountUsers");
        using (var context = new ConduitDbContext(_optionsBuilder.Options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var articleRepo = new ArticleRepository(context);

            var countWhoPrefer = await articleRepo.CountWhoFavoriteArticle(1);
            
            Assert.Equal(2,countWhoPrefer);
        }

    }
}