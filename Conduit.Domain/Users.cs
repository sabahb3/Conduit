namespace Conduit.Domain;

public class Users
{
    public Users()
    {
        Articles = new List<Articles>();
    }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string? Bio { get; set; }
    public string ProfilePicture { get; set; }

    public ICollection<Articles> Articles { get; set; }
}