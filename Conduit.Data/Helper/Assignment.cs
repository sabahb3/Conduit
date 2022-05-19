using Conduit.Domain;

namespace Conduit.Data.Helper;

public static class Assignment
{
    public static void AssignUser(this Users userToUpdate, Users user)
    {
        userToUpdate.Password = user.Password;
        userToUpdate.Email = user.Email;
        userToUpdate.Bio = user.Bio;
        userToUpdate.ProfilePicture = user.ProfilePicture;
    }

    public static void AssignArticle(this Articles articlesToUpdate, Articles articles)
    {
        articlesToUpdate.Title = articles.Title;
        articlesToUpdate.Description = articles.Description;
        articlesToUpdate.Body = articles.Body;
        articlesToUpdate.Date = articles.Date;
    }
}