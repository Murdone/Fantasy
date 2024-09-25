using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Fantasy.Frontend.AuthenticationProviders;

public class AuthenticationProviderTest : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var anonimous = new ClaimsIdentity();
        var user = new ClaimsIdentity(authenticationType: "test");
        var admin = new ClaimsIdentity(new List<Claim>
        {
            new Claim("FirstName", "Percibal"),
            new Claim("LastName", "Vasquez"),
            new Claim(ClaimTypes.Name, "pvasquez@yopmail.com"),
            new Claim(ClaimTypes.Role, "Admin") // Reclamo de rol
        },
        authenticationType: "test");

        return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(user)));
    }
}