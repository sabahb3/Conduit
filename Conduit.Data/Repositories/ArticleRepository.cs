using Conduit.Data.IRepositories;
using Conduit.Domain;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Data.Repositories;

public class ArticleRepository : IArticleRepository
{
    private readonly ConduitDbContext _context;

    public ArticleRepository(ConduitDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetCurrentArticleCount()
    {
        var currentUser = await _context.Articles.CountAsync();
        return currentUser;
    }

    public async Task CreateArticle(Articles createdArticle)
    {
        var user = _context.Users.Find(createdArticle.Username);
        if (user != null)
            await _context.Articles.AddAsync(createdArticle);
    }

    public async Task<int> Save()
    {
        var affected = await _context.SaveChangesAsync();
        return affected;
    }

    public async Task CreateArticles(List<Articles> createdArticles)
    {
        foreach (var article in createdArticles) await CreateArticle(article);
    }

    public async Task<Articles?> GetArticle(int articleId)
    {
        return await _context.Articles.FindAsync(articleId);
    }

    public async Task<List<Articles>> GetAllArticles()
    {
        return await _context.Articles.ToListAsync();
    }

    public async Task<Articles?> UpdateArticle(Articles updatedArticle)
    {
        var article = await _context.Articles.FindAsync(updatedArticle.Id);
        if (article != null)
        {
            article.Date = updatedArticle.Date;
            article.Title = updatedArticle.Title;
            article.Description = updatedArticle.Description;
            article.Body = updatedArticle.Body;
        }
        return article;
    }
}