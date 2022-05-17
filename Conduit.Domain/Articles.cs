namespace Conduit.Domain;

public class Articles
{
    public Articles()
    {
        Comments = new List<Comments>();
        UsersFavoriteArticles = new List<UsersFavoriteArticles>();
    }
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Body { get; set; }
    public DateTime Date { get; set; }

    public string Username { get; set; }
    public Users User { get; set; }

    public ICollection<Comments> Comments { get; set; }
    public ICollection<UsersFavoriteArticles> UsersFavoriteArticles { get; set; }
}