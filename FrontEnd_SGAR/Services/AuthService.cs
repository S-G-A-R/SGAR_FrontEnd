using FrontEnd_SGAR.Models;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace FrontEnd_SGAR.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;
        private string? _token;
        private bool _isAuthenticated = false;

        public AuthService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

        // LOGIN
        public async Task<string?> LoginAsync(CredencialesOrganizationRequest credenciales)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/organization/login", credenciales);

                if (!response.IsSuccessStatusCode)
                    return null;

                var token = await response.Content.ReadAsStringAsync();
                token = token.Trim('"'); // Quita comillas extras

                await SetTokenAsync(token);
                return token;
            }
            catch
            {
                return null;
            }
        }

        // Guardar token en memoria y localStorage
        public async Task SetTokenAsync(string token)
        {
            _token = token;
            await _js.InvokeVoidAsync("localStorage.setItem", "token", token);
        }

        // Obtener token desde memoria o localStorage
        public async Task<string?> GetTokenAsync()
        {
            if (!string.IsNullOrEmpty(_token))
                return _token;

            _token = await _js.InvokeAsync<string>("localStorage.getItem", "token");
            return _token;
        }

        // Configurar encabezado Authorization
        public async Task<bool> ConfigurarTokenAsync()
        {
            var token = await GetTokenAsync();

            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return true;
            }

            return false;
        }

        // Decodificar token para obtener claims
        public async Task<JwtSecurityToken?> ObtenerTokenDecodificado()
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            return jwt;
        }

        // Verificar si el token expiró
        public bool TokenExpirado(string token)
        {
            var jwt = new JwtSecurityToken(token);
            return jwt.ValidTo < DateTime.UtcNow;
        }

        // Cerrar sesión
        public async Task LogoutAsync()
        {
            _token = null;
            await _js.InvokeVoidAsync("localStorage.removeItem", "token");
            _http.DefaultRequestHeaders.Authorization = null;
        }
    }
}
