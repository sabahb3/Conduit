using Conduit.Domain;

namespace Conduit.Data.IRepositories;

public interface ICommentRepository
{
    public Task<int> GetCurrentCommentsCount();
    public Task CreateComment(Comments createdComment);
    public Task<int> Save();
}