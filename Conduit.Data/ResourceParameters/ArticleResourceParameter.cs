namespace Conduit.API.ResourceParameters;

public class ArticleResourceParameter
{
    public const int PageSize = 20;
    public string? Tag { get; set; }
    public string? Author { get; set; }
    public string? Favorited { get; set; }
    public int? Limit { get; set; } = 20;
    public int? Offset { get; set; } = 0;

    public bool IsFilteringNone()
    {
        return string.IsNullOrWhiteSpace(Tag) && string.IsNullOrWhiteSpace(Author) && string.IsNullOrWhiteSpace(Favorited);
    }
}