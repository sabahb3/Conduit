using Conduit.Domain;

namespace Conduit.Data.Repositories;

public class TagRepository
{
    public TagRepository(ConduitDbContext context)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Tags>> GetTags(int articleId)
    {
        throw new NotImplementedException();
    }
}