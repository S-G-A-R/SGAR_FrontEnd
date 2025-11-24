using FrontEnd_SGAR.DTOs;
using System.Net.Http.Json;

namespace FrontEnd_SGAR.Services
{
    public class TipoSuscripcionService
    {
        private readonly HttpClient _http;

        public TipoSuscripcionService(HttpClient http)
        {
            _http = http;
        }

        public async Task<TipoSuscripcionResponsePaginada?> ObtenerTiposSuscripcionAsync(int page = 0, int size = 10, string sortBy = "id", string direction = "asc")
        {
            try
            {
                var url = $"ApiVenta/api/tipos-suscripcion?page={page}&size={size}&sortBy={sortBy}&direction={direction}";
                var fullUrl = $"{_http.BaseAddress}{url}";
                var response = await _http.GetFromJsonAsync<TipoSuscripcionResponsePaginada>(url);
                return response;
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"[TipoSuscripcionService] Error HTTP: {httpEx.Message}");
                Console.WriteLine($"[TipoSuscripcionService] StatusCode: {httpEx.StatusCode}");
                return null;
            }
        }

        public async Task<TipoSuscripcionDTO?> ObtenerPorId(int id)
        {
            return await _http.GetFromJsonAsync<TipoSuscripcionDTO>($"ApiVenta/api/tipos-suscripcion/{id}");
        }

        public async Task<(bool Exito, string Mensaje)> EliminarTipoSuscripcionAsync(int id)
        {
            try
            {
                var url = $"ApiVenta/api/tipos-suscripcion/{id}";

                var response = await _http.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return (true, "Eliminado correctamente");
                }
                else
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    return (false, $"Error del servidor: {errorMsg}");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexión: {ex.Message}");
            }
        }

        public async Task<ResponseMessage?> CrearTipoSuscripcion(TipoSuscripcionRequest tp)
        {
            var response = await _http.PostAsJsonAsync("ApiVenta/api/tipos-suscripcion", tp);
            return await response.Content.ReadFromJsonAsync<ResponseMessage>();
        }

        public async Task<ResponseMessage?> ActualizarTipoSuscripcion(int id, TipoSuscripcionRequest tp)
        {
            var response = await _http.PutAsJsonAsync($"ApiVenta/api/tipos-suscripcion/{id}", tp);
            return await response.Content.ReadFromJsonAsync<ResponseMessage>();
        }

        public class ResponseMessage
        {
            public string? message { get; set; }
            public int id { get; set; }
        }

    }
}
