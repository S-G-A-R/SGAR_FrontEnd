using FrontEnd_SGAR.DTOs;
using System.Net.Http.Json;

namespace FrontEnd_SGAR.Services
{
    public class CategoriaService
    {
        private readonly HttpClient _http;

        public CategoriaService(HttpClient http)
        {
            _http = http;
        }

        public async Task<CategoriaProductoResponsePaginada?> ObtenerCategoriasAsync(int id, int page = 0, int size = 10)
        {
            try
            {
                var url = $"ApiVenta/api/categorias-productos/buscar/asociado/{id}?page={page}&size={size}";

                var response = await _http.GetFromJsonAsync<CategoriaProductoResponsePaginada>(url);
                return response;
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"[Categoria] Error HTTP: {httpEx.Message}");
                return null;
            }
        }

        public async Task<ResponseMessage?> CrearCategoria(CategoriaRequest categoria)
        {
            var response = await _http.PostAsJsonAsync("ApiVenta/api/categorias-productos", categoria);
            return await response.Content.ReadFromJsonAsync<ResponseMessage>();
        }

        public async Task<CategoriaProductoDTO?> ObtenerPorId(int id)
        {
            return await _http.GetFromJsonAsync<CategoriaProductoDTO>($"ApiVenta/api/categorias-productos/{id}");
        }

        public async Task<ResponseMessage?> ActualizarCategoria(int id, CategoriaRequest categoria)
        {
            var response = await _http.PutAsJsonAsync($"ApiVenta/api/categorias-productos/{id}", categoria);
            return await response.Content.ReadFromJsonAsync<ResponseMessage>();
        }

        public async Task<(bool Exito, string Mensaje)> EliminarCategoriaAsync(int id)
        {
            try
            {
                var url = $"ApiVenta/api/categorias-productos/{id}";

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
