using BusinessObjects.DTOs;
namespace Hosteland.Context
{
    public interface IUserContext
    {
        CurrentUserDTO GetCurrentUser(HttpContext context);
    }
}
