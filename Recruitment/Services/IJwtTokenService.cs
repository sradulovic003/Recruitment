using Recruitment.Infrastructure.Identity;

namespace Recruitment.API.Services
{
    public interface IJwtTokenService
    {
        Task<string> CreateTokenAsync(User user);
    }
}
