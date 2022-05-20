using Conduit.Domain;

namespace Conduit.Data.IRepositories;

public interface ICommentRepository
{
    public Task<int> GetCurrentCommentsCount();
    public Task CreateComment(Comments createdComment);
    public Task<int> Save();
    public Task CreateComments(List<Comments> createdComments);
    public Task<List<Comments>> ReadArticleComments(int articleId);
    public Task<List<Comments>> ReadUserComments(string username);
    public Task<Comments?> GetComment(int commentId);
    public Task<List<Comments>> GetComments();
    public Task DeleteComment(int commentId);
    public Task DeleteComments();
    public Task DeleteArticleComments(int articleId);
}