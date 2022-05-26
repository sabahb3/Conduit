using Conduit.Data.IRepositories;
using Conduit.Domain;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Data.Repositories;

public class TagRepository : ITagRepository
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

            article.ArticlesTags = await SetUpTags(tags);
        }
    }

    public async Task<List<ArticlesTags>> SetUpTags(List<Tags> tags)
    {
        List<ArticlesTags> articleTags = new List<ArticlesTags>();
        foreach (var tag in tags)
        {
            if (await _context.Tags.FirstOrDefaultAsync(t=>t.Tag==tag.Tag) == null)
            {
                _context.Tags.Add(tag);
            }
            articleTags.Add(new ArticlesTags
            {
                Tags = tag
            });
        }
        return articleTags;
    }

    public async Task<List<string>> GetPopularTag()
    {
        return await _context.Set<ArticlesTags>().GroupBy(a => a.Tag)
            .OrderByDescending(a => a.Count()).Take(4)
            .Select(t=>t.Key).ToListAsync();
    }
}