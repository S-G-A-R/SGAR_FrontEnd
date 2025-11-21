using Microsoft.JSInterop;
using System.Net.Http.Headers;

namespace FrontEnd_SGAR.Handlers
{
    /// <summary>
    /// Handler que intercepta todas las peticiones HTTP y agrega automáticamente
    /// el token JWT desde localStorage en la cabecera Authorization
    /// </summary>
    public class AuthenticationHandler : DelegatingHandler
    {
        private readonly IJSRuntime _jsRuntime;

        public AuthenticationHandler(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, 
            CancellationToken cancellationToken)
        {
            // Intentar obtener el token desde localStorage
            var token = await ObtenerTokenAsync();

            // Si existe un token, agregarlo a la cabecera Authorization
            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// Obtiene el token JWT almacenado en localStorage
        /// </summary>
        private async Task<string?> ObtenerTokenAsync()
        {
            try
            {
                var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "token");
                return token;
            }
            catch
            {
                // Si ocurre un error al acceder a localStorage, retornar null
                return null;
            }
        }
    }
}
