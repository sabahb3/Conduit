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
            var expected = new List<Tags>
            {
                new Tags
                {
                    Tag = "Greetings"
                },
                new Tags
                {
                    Tag = "Welcoming"
                }
            };
            
            var tags = await tagRepo.GetTags(1);
            
            Assert.Equal(expected,tags);

        }
    }

}