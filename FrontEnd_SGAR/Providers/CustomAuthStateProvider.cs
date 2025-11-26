using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FrontEnd_SGAR.Providers
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly JwtSecurityTokenHandler _tokenHandler = new();

        public CustomAuthStateProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "token");

                if (string.IsNullOrWhiteSpace(token))
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                // 1. Leer el token
                var jwtToken = _tokenHandler.ReadJwtToken(token);

                // 2. Validar si expiró
                if (jwtToken.ValidTo < DateTime.UtcNow)
                {
                    await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "token");
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                // 3. --- MAPEO MANUAL DE CLAIMS (LA SOLUCIÓN) ---
                var claimsIdentity = new ClaimsIdentity("jwt");

                foreach (var claim in jwtToken.Claims)
                {
                    // Si el claim es "role", lo agregamos como ClaimTypes.Role
                    if (claim.Type == "role")
                    {
                        claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, claim.Value));
                    }
                    // Si el claim es "unique_name" o "name", lo agregamos como ClaimTypes.Name
                    else if (claim.Type == "unique_name" || claim.Type == "name")
                    {
                        claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, claim.Value));
                    }
                    else
                    {
                        // Agregamos el resto de claims tal cual
                        claimsIdentity.AddClaim(claim);
                    }
                }

                var user = new ClaimsPrincipal(claimsIdentity);

                return new AuthenticationState(user);
            }
            catch
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        public void NotifyAuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task MarkUserAsAuthenticated(string token)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "token", token);
            NotifyAuthenticationStateChanged();
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "token"); // Usar removeItem es más seguro que clear
                                                                                  // await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "userId"); // Si guardas userId aparte
            NotifyAuthenticationStateChanged();
        }
    }
}
