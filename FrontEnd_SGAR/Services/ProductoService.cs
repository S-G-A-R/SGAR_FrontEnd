using FrontEnd_SGAR.DTOs;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using static FrontEnd_SGAR.Pages.Asociado.CrearProducto;

namespace FrontEnd_SGAR.Services
{
    public class ProductoService
    {
        private readonly HttpClient _http;

        public ProductoService(HttpClient http)
        {
            _http = http;
        }

        public async Task<CategoriaProductoResponsePaginada?> CargarCategoriasAsync(int id, int page = 0, int size = 10)
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

        public async Task<EmpresasResponsePaginada?> CargarEmpresasAsync(int id, int page = 0, int size = 10)
        {
            try
            {
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
        public async Task<string?> ObtenerImagenBase64Async(int id)
        {
            try
            {
                var url = $"ApiVenta/api/imagenes-productos/{id}/imagen";
                var imageResponse = await _http.GetAsync(url);

                if (imageResponse.IsSuccessStatusCode)
                {
                    var imageBytes = await imageResponse.Content.ReadAsByteArrayAsync();
                    return Convert.ToBase64String(imageBytes);
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ProductoService] Error al obtener imagen {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<int?> SubirImagenAsync(IBrowserFile archivo)
        {
            try
            {
                var content = new MultipartFormDataContent();
                // Límite de 10MB
                var fileContent = new StreamContent(archivo.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024));
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(archivo.ContentType);
                content.Add(fileContent, "imagen", archivo.Name);

                var response = await _http.PostAsync("ApiVenta/api/imagenes-productos", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    // Deserialización segura insensible a mayúsculas
                    var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var fotoElement = JsonSerializer.Deserialize<JsonElement>(jsonResponse, opciones);

                    if (fotoElement.TryGetProperty("id", out var idProp))
                    {
                        return idProp.GetInt32();
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Service] Error subiendo imagen: {ex.Message}");
                return null;
            }
        }

        public async Task<(bool Exito, string Mensaje)> GuardarProductosAsync(ProductoModeloValidacion modelo, IBrowserFile? archivoImagen, int id = 0)
        {
            try
            {
                // A. Si hay imagen nueva, subirla primero
                if (archivoImagen != null)
                {
                    var idFotoNueva = await SubirImagenAsync(archivoImagen);
                    if (idFotoNueva.HasValue)
                    {
                        modelo.IdFoto = idFotoNueva.Value;
                    }
                    else
                    {
                        return (false, "Error al subir la imagen. Intente nuevamente.");
                    }
                }

                // B. Preparar el MultipartFormData para el Producto
                var formData = new MultipartFormDataContent();

                formData.Add(new StringContent(modelo.Nombre), "nombre");
                formData.Add(new StringContent(modelo.Precio.ToString()), "precio");
                formData.Add(new StringContent(modelo.Tipo), "tipo");
                formData.Add(new StringContent(modelo.Descripcion ?? ""), "descripcion"); // Manejo de null

                // Enviar IDs
                formData.Add(new StringContent(modelo.CategoriaProductoId.ToString()), "categoriaProductoId");
                formData.Add(new StringContent(modelo.EmpresaId.ToString()), "empresaId");

                // Solo enviar IdFoto si tiene valor
                if (modelo.IdFoto.HasValue)
                {
                    formData.Add(new StringContent(modelo.IdFoto.Value.ToString()), "idFoto");
                }

                HttpResponseMessage response;
                bool esEdicion = id > 0;

                if (esEdicion)
                {
                    // CORRECCIÓN: Apuntar al endpoint de productos, no de vehículos
                    response = await _http.PutAsync($"ApiVenta/api/productos/{id}", formData);
                }
                else
                {
                    response = await _http.PostAsync("ApiVenta/api/productos", formData);
                }

                if (response.IsSuccessStatusCode)
                {
                    return (true, esEdicion ? "Producto actualizado exitosamente" : "Producto creado exitosamente");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return (false, $"Error del servidor: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error interno: {ex.Message}");
            }
        }
    }
}

