using FrontEnd_SGAR.DTOs;
using System.Net.Http.Json;

namespace FrontEnd_SGAR.Services
{
    public class PlanService
    {
        private readonly HttpClient _http;

        public PlanService(HttpClient http)
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

        public async Task<TipoSuscripcionDTO?> ObtenerTipoSuscripcionPorId(int id)
        {
            return await _http.GetFromJsonAsync<TipoSuscripcionDTO>($"ApiVenta/api/tipos-suscripcion/{id}");
        }

        public async Task<ResponseMessage?> CrearPlanSuscripcion(PlanRequest plan)
        {
            var response = await _http.PostAsJsonAsync("ApiVenta/api/planes-suscripcion", plan);
            return await response.Content.ReadFromJsonAsync<ResponseMessage>();
        }

        public class ResponseMessage
        {
            public string? message { get; set; }
        }


    }
}
