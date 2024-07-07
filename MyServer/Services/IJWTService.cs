using MyServer.Models;

namespace MyServer.Services
{
    public interface IJWTService
    {
        public string CreateJWT(User user);
    }
}
