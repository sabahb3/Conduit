using Conduit.Domain;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Data.Repositories;

public class ArticleRepository
{
    private readonly ConduitDbContext _context;

    public ArticleRepository(ConduitDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetCurrentArticleCount()
    {
        var currentUser= await _context.Articles.CountAsync();
        return currentUser;
    }

    public async Task CreateArticle(Articles createdArticle)
    {
        await _context.Articles.AddAsync(createdArticle);
    }

    public async Task<int> Save()
    {
        var affected=await _context.SaveChangesAsync();
        return affected;
    }
}