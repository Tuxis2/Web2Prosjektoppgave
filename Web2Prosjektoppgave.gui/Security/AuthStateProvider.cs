using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;
using Web2Prosjektoppgave.shared.Security;

namespace Web2Prosjektoppgave.gui.Security;

public class AuthStateProvider : AuthenticationStateProvider
{
    private readonly IJSRuntime _jsRuntime;

    public AuthStateProvider(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await GetJWT();
        var claims = JwtTokenHelper.DecodeJwt(token.TokenString);
            
        ClaimsIdentity claimsIdentity;
        if (claims.Any())
        {
            claimsIdentity = new ClaimsIdentity(claims, "Bearer");
        }
        else
        {
            claimsIdentity = new ClaimsIdentity();
        }
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        return new AuthenticationState(claimsPrincipal);
    }

    private async Task<Token> GetJWT()
    {
        var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken")
            .ConfigureAwait(false);

        var options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return JsonSerializer.Deserialize<Token>(json, options);
    }
}