using Conduit.Domain;

namespace Conduit.Data.IRepositories;

public interface ICommentRepository
{
    public Task<int> GetCurrentCommentsCount();
    public Task CreateComment(Comments createdComment);
    public Task<int> Save();
    public Task CreateComments(List<Comments> createdComments);
    public Task<List<Comments>> ReadArticleComments(int articleId);
    public Task<List<Comments>> ReadUserComment(string username);

}