using Project_PRN231_API.Models;

namespace Project_PRN231_API.Interface.Auth
{
    public interface IAuthService
    {
        string GetToken(User u);
    }
}
