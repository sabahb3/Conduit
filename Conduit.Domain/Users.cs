namespace Conduit.Domain;

public class Users
{
    public Users()
    {
        Articles = new List<Articles>();
        Comments = new List<Comments>();
        Followers = new List<Followers>();
        UsersFavoriteArticles = new List<UsersFavoriteArticles>();
    }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string? Bio { get; set; }
    public string ProfilePicture { get; set; }

    public ICollection<Articles> Articles { get; set; }
    public ICollection<Comments> Comments { get; set; }
    public ICollection<UsersFavoriteArticles> UsersFavoriteArticles { get; set; }
    public ICollection<Followers> Followers { get; set; }
}