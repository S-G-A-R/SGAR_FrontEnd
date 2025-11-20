using System.Net.Http.Json;
using static FrontEnd_SGAR.DTOs.OrganizacionDTO;

namespace FrontEnd_SGAR.Services
{
    public class OrganizacionService
    {
        private readonly HttpClient _http;

        public OrganizacionService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Organizacion>> ObtenerOrganizacionesAsync()
        {
            try
            {
                var baseUrl = _http.BaseAddress?.ToString() ?? "No BaseAddress";

                // Deserializar como respuesta paginada
                var response = await _http.GetFromJsonAsync<OrganizacionResponsePaginada>("ApiSeguridad/api/organization/list");

                var organizaciones = response?.Items ?? new List<Organizacion>();
                return organizaciones;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[OrganizacionService] Error: {ex.Message}");
                return new List<Organizacion>();
            }
        }
    }
}
