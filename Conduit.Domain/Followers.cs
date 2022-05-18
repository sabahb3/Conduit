namespace Conduit.Domain;

public class Followers
{
    public string Username { get; set; }
    public string FollowingId { get; set; }

    public Users User { get; set; }
    public Users Following { get; set; }
}