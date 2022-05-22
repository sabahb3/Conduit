using Conduit.Domain;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Data.Repositories;

public class TagRepository
{
    private readonly ConduitDbContext _context;

    public TagRepository(ConduitDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<string>> GetTags(int articleId)
    {
        var article = await _context.Articles.FindAsync(articleId);
        if (article != null)
        {
            await _context.Entry(article).Collection(a => a.ArticlesTags).LoadAsync();
            var articleTags= await _context.Articles.Select(a => a.ArticlesTags).FirstOrDefaultAsync();
            if (articleTags != null)
            {
                foreach (var tags in articleTags)
                {
                    await _context.Entry(tags).Reference(t => t.Tags).LoadAsync();
                }

                return articleTags.Select(a => a.Tag).OrderBy(a => a).ToList();
            }
        }
        return new List<string>();
    }

    public async Task<int> Save()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task AddTags(int articleId, List<Tags> tags)
    {
        var article = await _context.Articles.FindAsync(articleId);
        if (article!=null)
        {
            var articleTags =  tags.Select(t => new ArticlesTags
            {
                Tags = t
            }).ToList();
            article.ArticlesTags = articleTags;
        }
    }
}