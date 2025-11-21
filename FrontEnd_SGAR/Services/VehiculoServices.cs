using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using static FrontEnd_SGAR.DTOs.UserDTO;
using static FrontEnd_SGAR.DTOs.VehiculoDTO;
using static FrontEnd_SGAR.Pages.Organizacion.VehiculoFormulario;

namespace FrontEnd_SGAR.Services
{
    public class VehiculoServices
    {
        private readonly HttpClient _http;

        public VehiculoServices(HttpClient http)
        {
            _http = http;
        }

        /// <summary>
        /// Obtiene vehículos con paginación y filtros
        /// </summary>
        public async Task<VehiculoResponsePaginada?> ObtenerVehiculosAsync(
            int page = 0,
            int size = 10,
            string? placa = null,
            string? codigo = null,
            string? marcaId = null,
            string? tipoVehiculoId = null,
            string? estado = null,
            string? mecanico = null)
        {
            try
            {
                // Construir URL con el prefijo del gateway ApiAdmi
                var url = $"ApiAdmi/api/vehiculos?page={page}&size={size}&sort=id";
                
                if (!string.IsNullOrEmpty(placa))
                    url += $"&placa={Uri.EscapeDataString(placa)}";
                    
                if (!string.IsNullOrEmpty(codigo))
                    url += $"&codigo={Uri.EscapeDataString(codigo)}";
                    
                if (!string.IsNullOrEmpty(marcaId))
                    url += $"&marcaId={marcaId}";
                    
                if (!string.IsNullOrEmpty(tipoVehiculoId))
                    url += $"&tipoVehiculoId={tipoVehiculoId}";
                    
                if (!string.IsNullOrEmpty(estado))
                    url += $"&estado={estado}";
                    
                if (!string.IsNullOrEmpty(mecanico))
                    url += $"&mecanico={Uri.EscapeDataString(mecanico)}";

                Console.WriteLine($"[VehiculoService] URL: {url}");
                
                var response = await _http.GetFromJsonAsync<VehiculoResponsePaginada>(url);
                
                if (response != null)
                {
                    // Cargar imágenes para cada vehículo
                    foreach (var vehiculo in response.Items)
                    {
                        if (vehiculo.IdFoto.HasValue && vehiculo.IdFoto.Value > 0)
                        {
                            vehiculo.ImagenBase64 = await ObtenerImagenBase64Async(vehiculo.IdFoto.Value);
                        }
                    }
                }
                
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VehiculoService] Error al obtener vehículos: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Obtiene lista de marcas
        /// </summary>
        public async Task<List<MarcaDto>> ObtenerMarcasAsync()
        {
            try
            {
                var url = "ApiAdmi/api/marcas?size=1000";
                var response = await _http.GetAsync(url);
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    
                    if (jsonContent.Contains("\"content\""))
                    {
                        var pageResponse = JsonSerializer.Deserialize<JsonElement>(jsonContent);
                        if (pageResponse.TryGetProperty("content", out var contentArray))
                        {
                            return JsonSerializer.Deserialize<List<MarcaDto>>(contentArray.GetRawText(),
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
                        }
                    }
                    
                    return JsonSerializer.Deserialize<List<MarcaDto>>(jsonContent,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
                }
                
                return new List<MarcaDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VehiculoService] Error al obtener marcas: {ex.Message}");
                return new List<MarcaDto>();
            }
        }

        /// <summary>
        /// Obtiene lista de tipos de vehículo
        /// </summary>
        public async Task<List<TipoVehiculoDto>> ObtenerTiposVehiculoAsync()
        {
            try
            {
                var url = "ApiAdmi/api/tipos-vehiculos?size=1000";
                var response = await _http.GetAsync(url);
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    
                    if (jsonContent.Contains("\"content\""))
                    {
                        var pageResponse = JsonSerializer.Deserialize<JsonElement>(jsonContent);
                        if (pageResponse.TryGetProperty("content", out var contentArray))
                        {
                            return JsonSerializer.Deserialize<List<TipoVehiculoDto>>(contentArray.GetRawText(),
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
                        }
                    }
                    
                    return JsonSerializer.Deserialize<List<TipoVehiculoDto>>(jsonContent,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
                }
                
                return new List<TipoVehiculoDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VehiculoService] Error al obtener tipos de vehículo: {ex.Message}");
                return new List<TipoVehiculoDto>();
            }
        }

        /// <summary>
        /// Obtiene lista de operadores y sus nombres
        /// </summary>
        public async Task<Dictionary<int, string>> ObtenerOperadoresConNombresAsync()
        {
            try
            {
                var operadores = new Dictionary<int, string>();
                
                var url = "ApiSeguridad/api/operador/list?size=100";
                var operadorResponse = await _http.GetAsync(url);
                
                if (operadorResponse.IsSuccessStatusCode)
                {
                    var jsonContent = await operadorResponse.Content.ReadAsStringAsync();
                    var operadorData = JsonSerializer.Deserialize<JsonElement>(jsonContent);

                    if (operadorData.TryGetProperty("content", out var operadorContent) || 
                        operadorData.TryGetProperty("items", out operadorContent))
                    {
                        var operadoresList = JsonSerializer.Deserialize<List<OperadorDto>>(operadorContent.GetRawText(),
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();

                        foreach (var operador in operadoresList)
                        {
                            try
                            {
                                var userResponse = await _http.GetAsync($"ApiSeguridad/api/user/{operador.IdUser}");
                                if (userResponse.IsSuccessStatusCode)
                                {
                                    var userData = await userResponse.Content.ReadFromJsonAsync<UserDto>(
                                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                                    if (userData != null)
                                    {
                                        operadores[operador.Id] = $"{userData.Nombre} {userData.Apellido}".Trim();
                                    }
                                }
                            }
                            catch
                            {
                                operadores[operador.Id] = operador.CodigoOperador;
                            }
                        }
                    }
                }
                
                return operadores;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VehiculoService] Error al obtener operadores: {ex.Message}");
                return new Dictionary<int, string>();
            }
        }

        /// <summary>
        /// Carga imagen en Base64 para un vehículo
        /// </summary>
        public async Task<string?> ObtenerImagenBase64Async(int idFoto)
        {
            try
            {
                var url = $"ApiAdmi/api/fotos/{idFoto}/imagen";
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
                Console.WriteLine($"[VehiculoService] Error al obtener imagen {idFoto}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Elimina un vehículo
        /// </summary>
        public async Task<(bool Exito, string Mensaje)> EliminarVehiculoAsync(int id)
        {
            try
            {
                // 1. VERIFICA QUE ESTA RUTA SEA CORRECTA (ApiAdmi vs ApiAdmin)
                var url = $"ApiAdmi/api/vehiculos/{id}";

                var response = await _http.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return (true, "Eliminado correctamente");
                }
                else
                {
                    // Leemos qué nos dijo el servidor (ej: "Foreign Key constraint failed")
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    return (false, $"Error del servidor: {errorMsg}");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexión: {ex.Message}");
            }
        }

        // 1. Obtener un solo vehículo para edición
        public async Task<VehiculoFormModel?> ObtenerVehiculoParaEdicionAsync(int id)
        {
            try
            {
                var response = await _http.GetAsync($"ApiAdmi/api/vehiculos/{id}");
                if (response.IsSuccessStatusCode)
                {
                    // Usamos una clase DTO intermedia para deserializar y luego mapear al FormModel
                    var vehiculoDto = await response.Content.ReadFromJsonAsync<VehiculoDetalleDto>(
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (vehiculoDto == null) return null;

                    // Mapeo manual al modelo del formulario
                    return new VehiculoFormModel
                    {
                        IdMarca = (int)vehiculoDto.IdMarca,
                        Placa = vehiculoDto.Placa ?? "",
                        Codigo = vehiculoDto.Codigo ?? "",
                        IdTipoVehiculo = (int)vehiculoDto.IdTipoVehiculo,
                        Mecanico = vehiculoDto.Mecanico ?? "",
                        Taller = vehiculoDto.Taller ?? "",
                        Estado = vehiculoDto.Estado,
                        Descripcion = vehiculoDto.Descripcion ?? "",
                        IdOperador = vehiculoDto.IdOperador,
                        IdFoto = vehiculoDto.IdFoto
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Service] Error obteniendo vehículo {id}: {ex.Message}");
                return null;
            }
        }

        // 2. Subir imagen (Método privado auxiliar o público si lo necesitas fuera)
        public async Task<int?> SubirImagenAsync(IBrowserFile archivo)
        {
            try
            {
                var content = new MultipartFormDataContent();
                // Límite de 10MB configurado aquí
                var fileContent = new StreamContent(archivo.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024));
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(archivo.ContentType);
                content.Add(fileContent, "imagen", archivo.Name);

                var response = await _http.PostAsync("ApiAdmi/api/fotos", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var fotoElement = JsonSerializer.Deserialize<JsonElement>(jsonResponse);
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

        // 3. Guardar Vehículo (Orquesta la subida de imagen y el guardado del vehículo)
        public async Task<(bool Exito, string Mensaje)> GuardarVehiculoAsync(VehiculoFormModel modelo, IBrowserFile? archivoImagen, int id = 0)
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

                // B. Preparar el MultipartFormData para el Vehículo
                var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(modelo.IdMarca.ToString()), "idMarca");
                formData.Add(new StringContent(modelo.Placa), "placa");
                formData.Add(new StringContent(modelo.Codigo), "codigo");
                formData.Add(new StringContent(modelo.IdTipoVehiculo.ToString()), "idTipoVehiculo");
                formData.Add(new StringContent(modelo.Mecanico), "mecanico");
                formData.Add(new StringContent(modelo.Taller), "taller");
                formData.Add(new StringContent(modelo.Estado.ToString()), "estado");
                formData.Add(new StringContent(modelo.Descripcion), "descripcion");

                if (modelo.IdOperador.HasValue)
                    formData.Add(new StringContent(modelo.IdOperador.Value.ToString()), "idOperador");

                if (modelo.IdFoto.HasValue)
                    formData.Add(new StringContent(modelo.IdFoto.Value.ToString()), "idFoto");

                // C. Determinar si es POST (Crear) o PUT (Editar)
                HttpResponseMessage response;
                bool esEdicion = id > 0;

                if (esEdicion)
                {
                    response = await _http.PutAsync($"ApiAdmi/api/vehiculos/{id}", formData);
                }
                else
                {
                    response = await _http.PostAsync("ApiAdmi/api/vehiculos", formData);
                }

                if (response.IsSuccessStatusCode)
                {
                    return (true, esEdicion ? "Vehículo actualizado exitosamente" : "Vehículo creado exitosamente");
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
