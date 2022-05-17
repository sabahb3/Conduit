namespace Conduit.Domain;

public class Followers
{
    public string Username { get; set; }
    public string FollowerId { get; set; }

    public Users User { get; set; }
    public Users Follower { get; set; }
}