using FrontEnd_SGAR.DTOs;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        // Verifica si el asociado tiene un plan activo y retorna información del plan y días restantes
        public async Task<PlanVerificacionResult> VerificarPlanAsociadoAutenticadoAsync(int id, int page = 0, int size = 10)
        {
            try
            {
                var url = $"ApiVenta/api/planes-suscripcion/buscar/asociado/{id}?page={page}&size={size}";
                var resp = await _http.GetAsync(url);

                if (!resp.IsSuccessStatusCode)
                {
                    var txt = await resp.Content.ReadAsStringAsync();
                    return new PlanVerificacionResult { TienePlanActivo = false, Mensaje = $"Error al consultar planes: {resp.StatusCode} - {txt}" };
                }

                var json = await resp.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var pageResponse = JsonSerializer.Deserialize<PlanResponsePaginada>(json, opciones);

                if (pageResponse == null || pageResponse.Planes == null || !pageResponse.Planes.Any())
                {
                    return new PlanVerificacionResult { TienePlanActivo = false, Mensaje = "No se encontraron planes para el asociado." };
                }

                // Buscar un plan activo y con fechaFin en el futuro
                var now = DateTime.UtcNow;
                var planActivo = pageResponse.Planes.FirstOrDefault(p => p.activo && p.fechaFin > now);

                if (planActivo == null)
                {
                    return new PlanVerificacionResult { TienePlanActivo = false, Mensaje = "No hay plan activo." };
                }

                var diasRestantes = (int)Math.Ceiling((planActivo.fechaFin - now).TotalDays);
                if (diasRestantes < 0) diasRestantes = 0;

                return new PlanVerificacionResult
                {
                    TienePlanActivo = true,
                    DiasRestantes = diasRestantes,
                    Plan = planActivo,
                    Mensaje = "Plan activo encontrado."
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PlanService] Error verificando plan asociado: {ex.Message}");
                return new PlanVerificacionResult { TienePlanActivo = false, Mensaje = $"Error interno: {ex.Message}" };
            }
        }

        public class ResponseMessage
        {
            public string? message { get; set; }
        }

        private class PlanResponsePaginada
        {
            [JsonPropertyName("totalItems")] public int TotalItems { get; set; }
            [JsonPropertyName("totalPages")] public int TotalPages { get; set; }
            [JsonPropertyName("pageSize")] public int PageSize { get; set; }
            [JsonPropertyName("currentPage")] public int CurrentPage { get; set; }
            [JsonPropertyName("planes")] public List<PlanDetalleDTO>? Planes { get; set; }
        }

        public class PlanDetalleDTO
        {
            [JsonPropertyName("id")] public int id { get; set; }
            [JsonPropertyName("asociadoId")] public int asociadoId { get; set; }
            [JsonPropertyName("tipoSuscripcion")] public TipoSuscripcionDTO? tipoSuscripcion { get; set; }
            [JsonPropertyName("fechaInicio")] public DateTime fechaInicio { get; set; }
            [JsonPropertyName("fechaFin")] public DateTime fechaFin { get; set; }
            [JsonPropertyName("activo")] public bool activo { get; set; }
        }

        public class PlanVerificacionResult
        {
            public bool TienePlanActivo { get; set; }
            public int DiasRestantes { get; set; }
            public PlanDetalleDTO? Plan { get; set; }
            public string? Mensaje { get; set; }
        }

    }
}