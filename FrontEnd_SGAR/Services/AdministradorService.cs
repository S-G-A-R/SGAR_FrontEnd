using FrontEnd_SGAR.DTOs;
using System.Net.Http.Json;
using System.Text.Json;

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

        // Obtener usuario por ID
        public async Task<User?> ObtenerUsuarioPorIdAsync(int id)
        {
            try
            {
                return await _http.GetFromJsonAsync<User>($"ApiSeguridad/api/user/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Administrador] Error ObtenerUsuarioPorId: {ex.Message}");
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

        // Actualiza el rol de usuario usando el DTO que contiene Id e idRol
        public async Task<(bool Exito, string Mensaje)> ActualizarRolUsuarioAsync(DTOs.userRequestRol modelo)
        {
            try
            {
                var resp = await _http.PutAsJsonAsync($"ApiSeguridad/api/user/{modelo.Id}/role", modelo);
                if (resp.IsSuccessStatusCode)
                {
                    return (true, "Rol actualizado correctamente.");
                }

                var content = await resp.Content.ReadAsStringAsync();
                try
                {
                    var rm = JsonSerializer.Deserialize<ResponseMessage>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return (false, rm?.message ?? content);
                }
                catch
                {
                    return (false, content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Administrador] Error ActualizarRolUsuarioAsync: {ex.Message}");
                return (false, $"Error interno: {ex.Message}");
            }
        }

        public class ResponseMessage
        {
            public string? message { get; set; }
        }

    }
}
