using IdentityAuthApi.DTOs;
using IdentityAuthApi.Models;

namespace IdentityAuthApi.Service
{
    public interface IAuthService
    {
        public Task<AuthDTO> GenerateToken(AppUser user);
    }
}
