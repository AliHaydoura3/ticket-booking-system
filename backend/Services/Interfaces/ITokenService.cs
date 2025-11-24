using Backend.Models;

namespace Backend.Services
{
    public interface ITokenService
    {
        string GenerateToken(ApplicationUser user, IList<string> roles);
    }
}