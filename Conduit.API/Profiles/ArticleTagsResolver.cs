using AutoMapper;
using Conduit.Data.Models;
using Conduit.Domain;

namespace Conduit.API.Profiles;

public class ArticleTagsResolver : IValueResolver<ArticleForCreation,Articles,ICollection<ArticlesTags>>
{
    
    public ICollection<ArticlesTags> Resolve(ArticleForCreation source, Articles destination, ICollection<ArticlesTags> destMember, ResolutionContext context)
    {
        return source.Tags.Select(tag => new ArticlesTags { Tag = tag }).ToList();
    }
}