using System.Reflection.Metadata;
using Conduit.API.ResourceParameters;
using Conduit.Data.Helper;
using Conduit.Data.IRepositories;
using Conduit.Domain;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Data.Repositories;

public class ArticleRepository : IArticleRepository
{
    private readonly ConduitDbContext _context;

    public IQueryable<Articles> Articles
    {
        get
        {
            return _context.Articles;
        }
    }

    public ArticleRepository(ConduitDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetCurrentArticleCount()
    {
        var currentArticles = await _context.Articles.CountAsync();
        return currentArticles;
    }

    public async Task CreateArticle(Articles createdArticle)
    {
        var user = _context.Users.Find(createdArticle.Username);
        if (user == null) return;
        createdArticle.Date=DateTime.Now;
        createdArticle.UpdatingDate = createdArticle.Date;
        createdArticle.ArticlesTags = await AddTags(createdArticle);
        await _context.Articles.AddAsync(createdArticle);
    }

    private async Task<List<ArticlesTags>> AddTags(Articles createdArticle)
    {
        var articleTags = new List<ArticlesTags>();
        var tags = createdArticle.ArticlesTags.Select(t => t.Tag);
        foreach (var tag in tags)
        {
            if (await _context.Tags.FirstOrDefaultAsync(t=>t.Tag==tag.Trim()) == null)
            {
                _context.Tags.Add(new Tags{Tag = tag});
            }
            articleTags.Add(new ArticlesTags
            {
                Tag = tag
            });
        }
        return articleTags;
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

    public async Task<Articles?> GetArticle(string slug)
    {
        return await _context.Articles.FirstOrDefaultAsync(a => a.Title == slug);
    }

    public async Task<List<Articles>> GetAllArticles(IQueryable<Articles> articles, int offset, int limit)
    {
        return await articles.OrderByDescending(a=>a.Date).Skip(offset).Take(limit).ToListAsync();
    }

    public async Task<Articles?> UpdateArticle(Articles updatedArticle)
    {
        var article = await _context.Articles.FindAsync(updatedArticle.Id);
        article?.AssignArticle(updatedArticle);
        _context.Entry(article).State = EntityState.Modified;
        return article;
    }

    public async Task UpdateArticles(List<Articles> updatedArticles)
    {
        foreach (var article in updatedArticles)
        {
            await UpdateArticle(article);
        }
    }

    public async Task RemoveArticle(int articleId)
    {
        var article = await _context.Articles.FindAsync(articleId);
        if (article != null)
        {
            using (var deleteContext = new ConduitDbContext(_context.ConduitOptions.Options))
            {
                deleteContext.Articles.Remove(article);
            }

            _context.Entry(article).State = EntityState.Deleted;
        }
    }
    public async Task RemoveArticle(string slug, string username)
    {
        var article = await _context.Articles.FirstOrDefaultAsync(a=>a.Title==slug.Trim()&& a.Username==username.Trim());
        if (article != null)
        {
            using (var deleteContext = new ConduitDbContext(_context.ConduitOptions.Options))
            {
                deleteContext.Articles.Remove(article);
            }
            _context.Entry(article).State = EntityState.Deleted;
        }
    }

    public async Task FavoriteArticle(string username, int articleId)
    {
        var user = await _context.Users.FindAsync(username);
        if(user==null) return;
        var article = await _context.Articles.FindAsync(articleId);
        if (article == null) return;
        var isFavorite = await _context.Set<UsersFavoriteArticles>()
            .AnyAsync(a => a.Username == username && a.ArticleId == articleId);
        if (isFavorite) return;
        var favoriteArticle = new UsersFavoriteArticles
        {
            Username = username,
            ArticleId = articleId
        };
        _context.Entry(favoriteArticle).State = EntityState.Added;
        article.UsersFavoriteArticles.Add(favoriteArticle);
    }

    public async Task RemoveArticles()
    {
        foreach (var article in _context.Articles)
        {
            await RemoveArticle(article.Id);
        }
    }

    public async Task<int> CountWhoFavoriteArticle(int articleId)
    {
        var count =await _context.Articles.Where(a => a.Id == articleId).
            Select(a => a.UsersFavoriteArticles.Count).FirstOrDefaultAsync();
        return count;
    }

    public async Task<IEnumerable<Articles>> GetArticles(IQueryable<Articles> allArticles,ArticleResourceParameter articleResourceParameter)
    {
        if (articleResourceParameter == null)
            throw new ArgumentNullException();
        articleResourceParameter.Limit ??= ArticleResourceParameter.PageSize;
        articleResourceParameter.Offset ??= 0;
        if (articleResourceParameter.IsFilteringNone())
        {
            return await GetAllArticles(allArticles,articleResourceParameter.Offset.Value,articleResourceParameter.Limit.Value);
        }
        var articleCollection = allArticles.OrderByDescending(a=>a.Date) as IQueryable<Articles>;
        if (!string.IsNullOrWhiteSpace(articleResourceParameter.Tag))
        {
            var tag = articleResourceParameter.Tag.Trim();
            articleCollection = articleCollection.Where(a => a.ArticlesTags.Select(t=>t.Tag).Contains(tag));
        }

        if (!string.IsNullOrWhiteSpace(articleResourceParameter.Author))
        {
            var author = articleResourceParameter.Author.Trim();
            articleCollection = articleCollection.Where(a => a.Username == author);
        }

        if (!string.IsNullOrWhiteSpace(articleResourceParameter.Favorited))
        {
            var favorited = articleResourceParameter.Favorited.Trim();
            var articles = await GetFavorttedArticles(favorited);
            articleCollection = articleCollection.Where(a => articles.Contains(a.Id));
        }
        articleCollection = articleCollection.Skip(articleResourceParameter.Offset.Value)
            .Take(articleResourceParameter.Limit.Value);
        return await articleCollection.ToListAsync();
    }

    public async Task<IEnumerable<int>> GetFavorttedArticles(string usermae)
    {
       return _context.Set<UsersFavoriteArticles>().Where(s=>s.Username==usermae).Select(a => a.ArticleId);
    }

    public async Task<Users?> GetAuthor(int articleId)
    {
        var article= await _context.Articles.Include(a => a.User)
            .FirstOrDefaultAsync(a=>a.Id==articleId);
        if (article == null) return null;
        return article.User;
    }
    public async Task<bool> DoesFavoriteArticle(string username, int articleId)
    {
        return await _context.Set<UsersFavoriteArticles>()
            .AnyAsync(a => a.Username == username && a.ArticleId == articleId);
    }
    public async Task<IEnumerable<Articles>> GetFeedArticles(ArticleResourceParameter? articleResourceParameter,
        string username)
    {
        if (articleResourceParameter == null) throw new ArgumentNullException();
        var followed = _context.Set<Followers>().Where(f => f.Username == username).Select(f=>f.FollowingId);
        var articles =  _context.Articles.Where(a => followed.Contains(a.Username));
        return await GetArticles(articles, articleResourceParameter);
    }

    public async Task<Articles?> GetArticleBySlug(string slug)
    {
        var articleSlug = slug.Trim();
        return await _context.Articles.FirstOrDefaultAsync(s => s.Title == articleSlug);
    }
    public async Task<IEnumerable<string>> GetTags(int articleId)
    {
        var article = await _context.Articles.FindAsync(articleId);
        if (article == null) return new List<string>();
        _context.Entry(article).State = EntityState.Unchanged;
        await _context.Entry(article).Collection(a => a.ArticlesTags).LoadAsync();
        return article.ArticlesTags.Select(a => a.Tag).OrderBy(a=>a).ToList();
    }
}