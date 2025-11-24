using FrontEnd_SGAR.DTOs;
using System.Net.Http.Json;

namespace FrontEnd_SGAR.Services
{
    public class EmpresaService
    {
        private readonly HttpClient _http;
        public EmpresaService(HttpClient http)
        {
            _http = http;
        }

        public async Task<EmpresasResponsePaginada?> ObtenerEmpresasAsync(int id, int page = 0, int size = 10)
        {
            try
            {
                // La URL usa el ID que recibimos
                var url = $"ApiVenta/api/empresas/buscar/asociado/{id}?page={page}&size={size}";

                var response = await _http.GetFromJsonAsync<EmpresasResponsePaginada>(url);
                return response;
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"[empresas] Error HTTP: {httpEx.Message}");
                return null; 
            }
        }

        public async Task<ResponseMessage?> CrearEmpresa(EmpresaRequest em)
        {
            var response = await _http.PostAsJsonAsync("ApiVenta/api/empresas", em);
            return await response.Content.ReadFromJsonAsync<ResponseMessage>();
        }

        public async Task<EmpresaDTO?> ObtenerPorId(int id)
        {
            return await _http.GetFromJsonAsync<EmpresaDTO>($"ApiVenta/api/empresas/{id}");
        }

        public async Task<ResponseMessage?> ActualizarEmpresa(int id, EmpresaRequest em)
        {
            var response = await _http.PutAsJsonAsync($"ApiVenta/api/empresas/{id}", em);
            return await response.Content.ReadFromJsonAsync<ResponseMessage>();
        }

        public async Task<(bool Exito, string Mensaje)> EliminarEmpresaAsync(int id)
        {
            try
            {
                var url = $"ApiVenta/api/empresas/{id}";

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

        public class ResponseMessage
        {
            public string? message { get; set; }
        }
    }
}
