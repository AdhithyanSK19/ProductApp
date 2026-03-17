using Microsoft.AspNetCore.Identity;

namespace WalkersApp.Repository
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user,List<string> roles);
    }
}
