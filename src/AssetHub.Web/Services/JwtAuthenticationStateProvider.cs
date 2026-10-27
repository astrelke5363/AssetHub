using System.Security.Claims;
using AssetHub.Domain;
using Microsoft.AspNetCore.Components.Authorization;

namespace AssetHub.Web.Services;

public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var nameClaim = new Claim(ClaimTypes.Name, "Andrew Strelke");
        var emailClaim = new Claim(ClaimTypes.Email, "astrelke5363@gmail.com");
        var roleClaim = new Claim(ClaimTypes.Role, Roles.Administrator);
        // Parse the JWT token and create claims
        var claims = new List<Claim> { nameClaim, emailClaim, roleClaim };
        var identity = new ClaimsIdentity(claims, "jwt", ClaimTypes.Name, ClaimTypes.Role);
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public async Task SignOutAsync()
    {
        //TODO: Clear local tokens & change AuthenticationState to Anonymous role 
    }
}
