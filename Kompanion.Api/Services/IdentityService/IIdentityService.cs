using System.Threading.Tasks;
using Kompanion.Api.Models.Entities;

namespace Kompanion.Api.Services.IdentityService
{
    public interface IIdentityService
    {
        Task<User> Authenticate(string username, string password);
        User CreatePasswordHashAndSalt(User user, string password);
        int GetUserIdFromClaims();
        Task<string> CreateUserClaims(int userId);
    }
}