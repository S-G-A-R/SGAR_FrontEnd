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

        public async Task<ProductoResponsePaginada?> ObtenerProductoAsociadoAsync(int id, int page = 0, int size = 10)
        {
            try
            {
                // Corregir URL: agregar & entre asociadoId y page
                var url = $"ApiVenta/api/productos/buscar?asociadoId={id}&page={page}&size={size}&sortBy=id&direction=asc";

                Console.WriteLine($"[ProductoService] URL: {url}");

                var response = await _http.GetFromJsonAsync<ProductoResponsePaginada>(url);
                return response;
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"[producto] Error HTTP: {httpEx.Message}");
                return null;
            }
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
                using var content = new MultipartFormDataContent();
                using var stream = archivo.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024);
                using var fileContent = new StreamContent(stream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(archivo.ContentType);

                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "\"imagen\"", FileName = $"\"{archivo.Name}\"" };
                content.Add(fileContent, "imagen", archivo.Name);

                var response = await _http.PostAsync("ApiVenta/api/imagenes-productos", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    // Deserialización segura insensible a mayúsculas
                    var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var fotoElement = JsonSerializer.Deserialize<JsonElement>(jsonResponse, opciones);

                    if (fotoElement.ValueKind == JsonValueKind.Object && fotoElement.TryGetProperty("id", out var idProp))
                    {
                        if (idProp.TryGetInt32(out int idVal))
                            return idVal;
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

        public async Task<(bool Exito, string Mensaje)> EliminarProductoAsync(int id)
        {
            try
            {
                var productUrl = $"ApiVenta/api/productos/{id}";
                var imageDeleteUrl = $"ApiVenta/api/imagenes-productos/producto/{id}";

                try
                {
                    var imgResp = await _http.DeleteAsync(imageDeleteUrl);
                    if (!imgResp.IsSuccessStatusCode && imgResp.StatusCode != System.Net.HttpStatusCode.NotFound)
                    {
                        var imgErr = await imgResp.Content.ReadAsStringAsync();
                        Console.WriteLine($"[ProductoService] Advertencia al eliminar imagen del producto {id}: {imgResp.StatusCode} - {imgErr}");
                    }
                }
                catch (Exception exImg)
                {
                    Console.WriteLine($"[ProductoService] Error al intentar eliminar imagen del producto {id}: {exImg.Message}");
                }

                var response = await _http.DeleteAsync(productUrl);


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

        public async Task<(bool Exito, string Mensaje)> GuardarProductosAsync(ProductoModeloValidacion modelo, IBrowserFile? archivoImagen, int id = 0)
        {
            try
            {
                if (archivoImagen != null)
                {
                    var idFotoNueva = await SubirImagenAsync(archivoImagen);
                    if (idFotoNueva.HasValue)
                    {
                        modelo.FotoId = idFotoNueva.Value;
                    }
                    else
                    {
                        return (false, "Error al subir la imagen. Intente nuevamente.");
                    }
                }

                var payload = new
                {
                    nombre = modelo.Nombre ?? string.Empty,
                    precio = modelo.Precio,
                    tipo = modelo.Tipo ?? string.Empty,
                    descripcion = modelo.Descripcion ?? string.Empty,
                    categoriaProductoId = modelo.CategoriaProductoId,
                    empresaId = modelo.EmpresaId,
                    fotoId = modelo.FotoId 
                };

                HttpResponseMessage response;
                bool esEdicion = id > 0;

                if (esEdicion)
                {
                    response = await _http.PutAsJsonAsync($"ApiVenta/api/productos/{id}", payload);
                }
                else
                {
                    response = await _http.PostAsJsonAsync("ApiVenta/api/productos", payload);
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

