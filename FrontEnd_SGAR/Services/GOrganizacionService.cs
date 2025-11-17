using System.Collections.Generic;
using System.Net.Http.Json;

namespace FrontEnd_SGAR.Services
{
    public class GOrganizacionService
    {
        private readonly HttpClient _http;

        public GOrganizacionService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<OrganizacionDTO>> ObtenerOrganizaciones(int page = 1, int pageSize = 10)
        {
            return await _http.GetFromJsonAsync<List<OrganizacionDTO>>(
                $"api/organization/list?page={page}&pageSize={pageSize}"
            ) ?? new List<OrganizacionDTO>();
        }


        public async Task<OrganizacionDTO?> ObtenerPorId(int id)
        {
            return await _http.GetFromJsonAsync<OrganizacionDTO>($"api/organization/{id}");
        }

        public async Task<ResponseMessage?> Crear(OrganizationRequest org)
        {
            var response = await _http.PostAsJsonAsync("api/organization", org);
            return await response.Content.ReadFromJsonAsync<ResponseMessage>();
        }

        public async Task<ResponseMessage?> Actualizar(int id, OrganizationRequest org)
        {
            var response = await _http.PutAsJsonAsync($"api/organization/{id}", org);
            return await response.Content.ReadFromJsonAsync<ResponseMessage>();
        }

        public async Task<bool> Eliminar(int id)
        {
            var response = await _http.DeleteAsync($"api/organization/{id}");
            return response.IsSuccessStatusCode;
        }
    }

    public class OrganizacionDTO
    {
        public int id { get; set; }
        public string nombreOrganizacion { get; set; } = "";
        public string telefono { get; set; } = "";
        public string email { get; set; } = "";
        public string idMunicipio { get; set; } = "";
    }

    public class OrganizationRequest
    {
        public string nombreOrganizacion { get; set; } = "";
        public string telefono { get; set; } = "";
        public string email { get; set; } = "";
        public string password { get; set; } = "";
        public string idMunicipio { get; set; } = "";
        public string notificacion { get; set; } = "";
        public int idRol { get; set; } = 5;
    }

    public class ResponseMessage
    {
        public string? message { get; set; }
        public int id { get; set; }
    }
}

