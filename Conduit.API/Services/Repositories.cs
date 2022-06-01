using Conduit.Data.IRepositories;
using Conduit.Data.Repositories;

namespace Microsoft.Extensions.DependencyInjection;

public static class Repositories
{
    public static IServiceCollection AddWebRepositories(this IServiceCollection services)
    {
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IArticleRepository, ArticleRepository>();
        services.AddTransient<ITagRepository, TagRepository>();
        services.AddTransient<ICommentRepository, CommentRepository>();
        return services;
    }
}