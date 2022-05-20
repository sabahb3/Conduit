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
}