using System.Net.Http.Json;
using FrontEnd_SGAR.DTOs;

namespace FrontEnd_SGAR.Services
{
    public class TipoSuscripcionService
    {
        private readonly HttpClient _http;

        public TipoSuscripcionService(HttpClient http)
        {
            _http = http;
        }

        /// <summary>
        /// Obtiene la lista de tipos de suscripción con paginación a través del API Gateway
        /// </summary>
        public async Task<TipoSuscripcionResponsePaginada?> ObtenerTiposSuscripcionAsync(int page = 0, int size = 10, string sortBy = "id", string direction = "asc")
        {
            try
            {
                var url = $"ApiVenta/api/tipos-suscripcion?page={page}&size={size}&sortBy={sortBy}&direction={direction}";
                var fullUrl = $"{_http.BaseAddress}{url}";
                var response = await _http.GetFromJsonAsync<TipoSuscripcionResponsePaginada>(url);
                
                if (response != null)
                {
                    Console.WriteLine($"[TipoSuscripcionService] Tipos de suscripción obtenidos: {response.TiposSuscripcion?.Count ?? 0}");
                    Console.WriteLine($"[TipoSuscripcionService] Total elementos: {response.TotalItems}");
                    Console.WriteLine($"[TipoSuscripcionService] Total páginas: {response.TotalPages}");
                }
                else
                {
                    Console.WriteLine("[TipoSuscripcionService] Response es NULL");
                }
                
                return response;
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"[TipoSuscripcionService] Error HTTP: {httpEx.Message}");
                Console.WriteLine($"[TipoSuscripcionService] StatusCode: {httpEx.StatusCode}");
                return null;
            }
        }
    }
}
