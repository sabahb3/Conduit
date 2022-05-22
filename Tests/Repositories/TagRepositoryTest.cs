using Conduit.Data;
using Conduit.Data.Repositories;
using Conduit.Domain;
using Microsoft.EntityFrameworkCore;

namespace Tests.Repositories;

public class TagRepositoryTest
{
    private readonly DbContextOptionsBuilder<ConduitDbContext> _optionsBuilder;

    public TagRepositoryTest()
    {
        _optionsBuilder = new DbContextOptionsBuilder<ConduitDbContext>();
    }

    [Fact]
    public async Task ShouldGiveArticleTag()
    {
        _optionsBuilder.UseInMemoryDatabase("ArticleTage");
        using (var context = new ConduitDbContext(_optionsBuilder.Options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var tagRepo = new TagRepository(context);
            var expected = new List<string>
            {
                "Greetings",
                "Welcoming"
                };
            
            var tags = await tagRepo.GetTags(1);
            
            Assert.Equal(expected,tags);
        }
    }

    [Fact]
    public async Task ShouldAddArticleTags()
    {
        _optionsBuilder.UseInMemoryDatabase("ArticleTage");
        using (var context = new ConduitDbContext(_optionsBuilder.Options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var tagRepo = new TagRepository(context);
            var tagsToAdd = new List<Tags>
            {
                new Tags
                {
                    Tag = "Test1"
                }
            };
            
            var oldTags = await tagRepo.GetTags(1);
            await tagRepo.AddTags(1, tagsToAdd);
            await tagRepo.Save();
            var newTags = await tagRepo.GetTags(1);
            
            Assert.NotEqual(oldTags,newTags);
            Assert.Single(newTags);

        }
        
    }

    [Fact]
    public async Task ShouldGiveMostPopularTags()
    {
        _optionsBuilder.UseInMemoryDatabase("ArticleTage");
        using (var context = new ConduitDbContext(_optionsBuilder.Options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var tagRepo = new TagRepository(context);
            var expected = new List<string>
            {
                "Greetings",
                "Welcoming"
            };
            var actual = await tagRepo.GetPopularTag();
            Assert.Equal(expected,actual);
        }
        
    }
}