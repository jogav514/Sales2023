using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Sales.WEB.Auth
{
    public class AuthenticationProviderTest : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {

            var anonimous = new ClaimsIdentity();
            var jegoUser = new ClaimsIdentity(new List<Claim>
        {
            new Claim("FirstName", "Johan"),
            new Claim("LastName", "Gaviria"),
            new Claim(ClaimTypes.Name, "jego@yopmail.com"),
            new Claim(ClaimTypes.Role, "Admin")

        }, authenticationType: "test");

            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(jegoUser)));
        }

    }
}
