using Conduit.Data.IRepositories;
using Conduit.Domain;

namespace Conduit.Data.Repositories;

public class CommentRepository :ICommentRepository
{
    public CommentRepository(ConduitDbContext context)
    {
        throw new NotImplementedException();
    }

    public async Task<int> GetCurrentCommentsCount()
    {
        throw new NotImplementedException();
    }

    public async Task CreateComment(Comments createdComment)
    {
        throw new NotImplementedException();
    }

    public async Task<int> Save()
    {
        throw new NotImplementedException();
    }
}