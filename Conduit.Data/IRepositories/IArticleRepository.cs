using Conduit.API.ResourceParameters;
using Conduit.Domain;

namespace Conduit.Data.IRepositories;

public interface IArticleRepository
{
    public Task<int> GetCurrentArticleCount();
    public Task CreateArticle(Articles createdArticle);
    public Task<int> Save();
    public Task CreateArticles(List<Articles> createdArticles);
    public Task<Articles?> GetArticle(int articleId);
    public Task<List<Articles>> GetAllArticles(int offset, int limit);
    public Task<IEnumerable<Articles>> GetArticles(ArticleResourceParameter articleResourceParameter);
    public Task<Articles?> UpdateArticle(Articles updatedArticle);
    public Task UpdateArticles(List<Articles> updatedArticles);
    public  Task RemoveArticle(int articleId);
    public Task RemoveArticles();
    public Task<int> CountWhoFavoriteArticle(int articleId);
    public Task<Users?> GetAuthor(int articleId);
}