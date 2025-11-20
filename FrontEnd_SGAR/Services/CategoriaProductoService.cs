using SgarApiVenta.Client.Models;
using System.Net.Http.Json;

namespace SgarApiVenta.Client.Services
{
    public class CategoriaProductoService
    {
        private readonly HttpClient _httpClient;
        private const string ApiEndpoint = "api/categorias-productos";

        public CategoriaProductoService(IHttpClientFactory httpClientFactory)
        {
            // Obtener el cliente "VentasAPI" que ya tiene adjunto el AuthenticationHeaderHandler.
            _httpClient = httpClientFactory.CreateClient("VentasAPI");
        }

        
        // -----------------------------------------------------
        //   GET — Obtener categorías (CRUD: Read)
        // -----------------------------------------------------

        public async Task<PaginatedResponse<CategoriaProducto>?> GetCategoriasAsync(
            int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                string url = $"{ApiEndpoint}?pagina={pageNumber}&limite={pageSize}";
                // El token se adjunta aquí de forma transparente
                return await _httpClient.GetFromJsonAsync<PaginatedResponse<CategoriaProducto>>(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener categorías: {ex.Message}");
                return null;
            }
        }

        public async Task<CategoriaProducto?> GetCategoriaByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<CategoriaProducto>($"{ApiEndpoint}/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener categoría {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<List<CategoriaProducto>?> BuscarPorNombreAsync(string nombre)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<CategoriaProducto>>(
                    $"{ApiEndpoint}/buscar/nombre?nombre={nombre}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar categoría por nombre: {ex.Message}");
                return null;
            }
        }

        // -----------------------------------------------------
        //   POST — Crear categoría (CRUD: Create)
        // -----------------------------------------------------

        public async Task<CategoriaProducto?> CreateCategoriaAsync(CategoriaProducto nuevaCategoria)
        {
            try
            {
                // El token se adjunta aquí de forma transparente
                var response = await _httpClient.PostAsJsonAsync(ApiEndpoint, nuevaCategoria);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CategoriaProducto>();
                }

                Console.WriteLine($"Error al crear categoría: {response.StatusCode}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al crear categoría: {ex.Message}");
                return null;
            }
        }

        // -----------------------------------------------------
        //   PUT — Actualizar categoría (CRUD: Update)
        // -----------------------------------------------------

        public async Task<bool> UpdateCategoriaAsync(int id, CategoriaProducto categoria)
        {
            try
            {
                // El token se adjunta aquí de forma transparente
                var response = await _httpClient.PutAsJsonAsync($"{ApiEndpoint}/{id}", categoria);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al actualizar categoría: {ex.Message}");
                return false;
            }
        }

        // -----------------------------------------------------
        //   DELETE — Eliminar categoría (CRUD: Delete)
        // -----------------------------------------------------

        public async Task<bool> EliminarCategoriaAsync(int id)
        {
            try
            {
                // El token se adjunta aquí de forma transparente
                var response = await _httpClient.DeleteAsync($"{ApiEndpoint}/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al eliminar categoría: {ex.Message}");
                return false;
            }
        }
    }
}