using Conduit.Data.IRepositories;
using Conduit.Domain;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Data.Repositories;

public class CommentRepository :ICommentRepository
{
    private readonly ConduitDbContext _context;

    public CommentRepository(ConduitDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetCurrentCommentsCount()
    {
        return await _context.Comments.CountAsync();
    }

    public async Task CreateComment(Comments createdComment)
    {
        var user = await _context.Users.FindAsync(createdComment.Username);
        var article = await _context.Articles.FindAsync(createdComment.ArticlesId);
        if (user != null && article != null) 
            await _context.Comments.AddAsync(createdComment);
    }

    public async Task<int> Save()
    {
        var affected = await _context.SaveChangesAsync();
        return affected;
    }

    public async Task CreateComments(List<Comments> createdComments)
    {
        foreach (var comment in createdComments) await CreateComment(comment);

    }

    public async Task<List<Comments>> ReadArticleComments(int articleId)
    {
        return await _context.Comments.Where(c => c.ArticlesId == articleId).ToListAsync();
    }

    public async Task<List<Comments>> ReadUserComments(string username)
    {
        return await _context.Comments.Where(c => c.Username == username).ToListAsync();
    }

    public async Task<Comments?> GetComment(int commentId)
    {
        return await _context.Comments.FindAsync(commentId);
    }

    public async Task<List<Comments>> GetComments()
    {
        return await _context.Comments.ToListAsync();
    }

    public async Task DeleteComment(int commentId)
    {
        var comment = await _context.Comments.FindAsync(commentId);
        if (comment != null)
        {
            using (var deleteContext = new ConduitDbContext(_context.ConduitOptions.Options))
            {
                deleteContext.Comments.Remove(comment);
            }
            _context.Entry(comment).State = EntityState.Deleted;
        }
    }

    public async Task DeleteComments()
    {
        foreach (var comment in _context.Comments)
        {
            await DeleteComment(comment.Id);
        }
    }
}