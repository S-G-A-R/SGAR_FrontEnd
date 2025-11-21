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

        /// <summary>
        /// Obtiene todas las organizaciones con paginación
        /// </summary>
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

        /// <summary>
        /// Obtiene todas las organizaciones con paginación
        /// </summary>
        public async Task<OrganizacionResponsePaginada?> ObtenerOrganizacionesAsync(int page = 1, int pageSize = 10)
        {
            try
            {
                var response = await _http.GetFromJsonAsync<OrganizacionResponsePaginada>(
                    $"ApiSeguridad/api/organization/list?page={page}&pageSize={pageSize}");
                
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[OrganizacionService] Error: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Busca organizaciones con filtros
        /// </summary>
        public async Task<OrganizacionResponsePaginada?> BuscarOrganizacionesAsync(
            string? nombreOrganizacion = null,
            string? telefono = null,
            string? email = null,
            string? idMunicipio = null,
            int page = 1,
            int pageSize = 10)
        {
            try
            {
                var query = $"nombreOrganizacion={nombreOrganizacion}&telefono={telefono}&email={email}&idMunicipio={idMunicipio}&page={page}&pageSize={pageSize}";
                
                var response = await _http.GetFromJsonAsync<OrganizacionResponsePaginada>(
                    $"ApiSeguridad/api/organization/search?{query}");
                
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[OrganizacionService] Error al buscar: {ex.Message}");
                return null;
            }
        }

        public async Task<OrganizacionDetalle?> ObtenerPorId(int id)
        {
            return await _http.GetFromJsonAsync<OrganizacionDetalle>($"ApiSeguridad/api/organization/{id}");
        }

        public async Task<ResponseMessage?> Crear(OrganizationRequest org)
        {
            var response = await _http.PostAsJsonAsync("ApiSeguridad/api/organization", org);
            return await response.Content.ReadFromJsonAsync<ResponseMessage>();
        }

        public async Task<ResponseMessage?> Actualizar(int id, OrganizationRequest org)
        {
            var response = await _http.PutAsJsonAsync($"ApiSeguridad/api/organization/{id}", org);
            return await response.Content.ReadFromJsonAsync<ResponseMessage>();
        }

        public async Task<bool> Eliminar(int id)
        {
            var response = await _http.DeleteAsync($"ApiSeguridad/api/organization/{id}");
            return response.IsSuccessStatusCode;
        }

        public class ResponseMessage
        {
            public string? message { get; set; }
            public int id { get; set; }
        }
    }
}
