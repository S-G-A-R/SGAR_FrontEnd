using FrontEnd_SGAR.DTOs;
using System.Net.Http.Json;

namespace FrontEnd_SGAR.Services
{
    public class AdministradorService
    {

        private readonly HttpClient _http;

        public AdministradorService(HttpClient http)
        {
            _http = http;
        }

        // Devuelve la respuesta paginada completa para que la UI pueda mostrar paginación
        public async Task<UserResponsePaginada?> ObtenerUsuarioAsync(int page = 0, int pageSize = 10)
        {
            try
            {
                var response = await _http.GetFromJsonAsync<UserResponsePaginada>($"ApiSeguridad/api/user/list?page={page}&pageSize={pageSize}");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Administrador] Error: {ex.Message}");
                return null;
            }
        }

        public async Task<List<Rol>> ObtenerRolAsync()
        {
            try
            {
                var response = await _http.GetFromJsonAsync<List<Rol>>("ApiSeguridad/api/rol");
                return response ?? new List<Rol>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Rol] Error: {ex.Message}");
                return new List<Rol>();
            }
        }


    }
}
