using BusinessObjects.DTOs;
namespace HostelandAuthorization.Context
{
    public interface IUserContext
    {
        CurrentUserDTO GetCurrentUser(HttpContext context);
    }
}
