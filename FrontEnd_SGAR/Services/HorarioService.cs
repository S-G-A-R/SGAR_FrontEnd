using System.Net.Http.Headers;
using System.Text.Json;
using System.Web;
using Microsoft.JSInterop;

namespace FrontEnd_SGAR.Services
{
    public class HorarioService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IJSRuntime _js;

        public HorarioService(IHttpClientFactory clientFactory, IJSRuntime js)
        {
            _clientFactory = clientFactory;
            _js = js;
        }

        private async Task<HttpClient> ObtenerClienteConToken()
        {
            var client = _clientFactory.CreateClient("JavaAPI");
            var token = await _js.InvokeAsync<string>("localStorage.getItem", "token");

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }

        public async Task<PaginacionHorarioResponse> ObtenerHorariosAsync(FiltroHorario filtros)
        {
            try
            {
                var client = await ObtenerClienteConToken();
                var query = HttpUtility.ParseQueryString(string.Empty);

                // Parámetros según tu API Java
                if (filtros.Organizacion > 0)
                    query["organizacion"] = filtros.Organizacion.ToString();

                if (!string.IsNullOrWhiteSpace(filtros.Dia))
                    query["dia"] = filtros.Dia;

                if (!string.IsNullOrWhiteSpace(filtros.ZonaId))
                    query["zonaId"] = filtros.ZonaId;

                if (filtros.Turno > 0)
                    query["turno"] = filtros.Turno.ToString();

                // Paginación (Spring Boot usa page 0-based)
                query["page"] = (filtros.Page - 1).ToString();
                query["size"] = filtros.Size.ToString();

                string url = $"api/horarios?{query}";
                Console.WriteLine($"🔍 URL: {url}");

                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"❌ Error: {response.StatusCode}");
                    return new PaginacionHorarioResponse();
                }

                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"📄 JSON Response: {json.Substring(0, Math.Min(300, json.Length))}...");

                // Deserializar usando JsonElement para manejar tipos dinámicos
                var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);

                var items = new List<HorarioDTO>();

                if (jsonElement.TryGetProperty("content", out var contentArray))
                {
                    foreach (var item in contentArray.EnumerateArray())
                    {
                        var horario = new HorarioDTO();

                        if (item.TryGetProperty("id", out var idProp))
                            horario.Id = idProp.GetInt32();

                        if (item.TryGetProperty("dia", out var diaProp))
                            horario.Dia = diaProp.GetString() ?? "";

                        if (item.TryGetProperty("zonaId", out var zonaProp))
                            horario.ZonaId = zonaProp.GetString() ?? "";

                        if (item.TryGetProperty("idOrganizacion", out var orgProp))
                            horario.IdOrganizacion = orgProp.GetInt32();

                        if (item.TryGetProperty("turno", out var turnoProp))
                            horario.Turno = (byte)turnoProp.GetInt32();

                        // Manejar las horas que pueden venir como string o LocalTime
                        if (item.TryGetProperty("horaEntrada", out var horaEntradaProp))
                        {
                            if (horaEntradaProp.ValueKind == JsonValueKind.String)
                            {
                                var timeStr = horaEntradaProp.GetString();
                                if (TimeSpan.TryParse(timeStr, out var timeSpan))
                                {
                                    horario.HoraEntrada = timeSpan.ToString(@"hh\:mm");
                                }
                            }
                            else if (horaEntradaProp.ValueKind == JsonValueKind.Array)
                            {
                                // LocalTime viene como array [hora, minuto, segundo]
                                var timeArray = horaEntradaProp.EnumerateArray().ToArray();
                                if (timeArray.Length >= 2)
                                {
                                    var hora = timeArray[0].GetInt32();
                                    var minuto = timeArray[1].GetInt32();
                                    horario.HoraEntrada = $"{hora:D2}:{minuto:D2}";
                                }
                            }
                        }

                        if (item.TryGetProperty("horaSalida", out var horaSalidaProp))
                        {
                            if (horaSalidaProp.ValueKind == JsonValueKind.String)
                            {
                                var timeStr = horaSalidaProp.GetString();
                                if (TimeSpan.TryParse(timeStr, out var timeSpan))
                                {
                                    horario.HoraSalida = timeSpan.ToString(@"hh\:mm");
                                }
                            }
                            else if (horaSalidaProp.ValueKind == JsonValueKind.Array)
                            {
                                // LocalTime viene como array [hora, minuto, segundo]
                                var timeArray = horaSalidaProp.EnumerateArray().ToArray();
                                if (timeArray.Length >= 2)
                                {
                                    var hora = timeArray[0].GetInt32();
                                    var minuto = timeArray[1].GetInt32();
                                    horario.HoraSalida = $"{hora:D2}:{minuto:D2}";
                                }
                            }
                        }

                        items.Add(horario);
                    }
                }

                // Obtener metadatos de paginación
                var totalPages = jsonElement.TryGetProperty("totalPages", out var totalPagesProp) ? totalPagesProp.GetInt32() : 0;
                var totalElements = jsonElement.TryGetProperty("totalElements", out var totalElementsProp) ? totalElementsProp.GetInt32() : 0;
                var currentPageNumber = jsonElement.TryGetProperty("number", out var numberProp) ? numberProp.GetInt32() : 0;
                var isLast = jsonElement.TryGetProperty("last", out var lastProp) ? lastProp.GetBoolean() : true;
                var isFirst = jsonElement.TryGetProperty("first", out var firstProp) ? firstProp.GetBoolean() : true;

                return new PaginacionHorarioResponse
                {
                    Items = items,
                    TotalPages = totalPages,
                    TotalCount = totalElements,
                    CurrentPage = currentPageNumber + 1, // Convertir a 1-based
                    HasNextPage = !isLast,
                    HasPreviousPage = !isFirst
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Error completo: {ex}");
                return new PaginacionHorarioResponse();
            }
        }

        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                var client = await ObtenerClienteConToken();
                var response = await client.DeleteAsync($"api/horarios/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Error eliminando: {ex.Message}");
                return false;
            }
        }
    }

    // DTOs simplificados
    public class HorarioDTO
    {
        public int Id { get; set; }
        public string Dia { get; set; } = "";
        public string ZonaId { get; set; } = "";
        public byte Turno { get; set; }
        public int IdOrganizacion { get; set; }
        public string HoraEntrada { get; set; } = "";
        public string HoraSalida { get; set; } = "";
    }

    public class FiltroHorario
    {
        public int Organizacion { get; set; }
        public string Dia { get; set; } = "";
        public string ZonaId { get; set; } = "";
        public byte Turno { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
    }

    public class PaginacionHorarioResponse
    {
        public List<HorarioDTO> Items { get; set; } = new();
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }
}