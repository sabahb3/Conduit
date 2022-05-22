using Conduit.Domain;

namespace Conduit.Data.IRepositories;

public interface ITagRepository
{
    public Task<IEnumerable<Tags>> GetTags(int articleId);

}