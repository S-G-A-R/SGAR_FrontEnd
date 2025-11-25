using System.Net.Http.Json;
using System.Text.Json;
using System.Web;
using FrontEnd_SGAR.DTOs;

namespace FrontEnd_SGAR.Services
{
    public class HorarioService
    {
        private readonly HttpClient _http;
        private readonly ZonaService _zonaService;
        private readonly OrganizacionService _organizacionService;

        public HorarioService(HttpClient http, ZonaService zonaService, OrganizacionService organizacionService)
        {
            _http = http;
            _zonaService = zonaService;
            _organizacionService = organizacionService;
        }

        public async Task<PaginacionHorarioResponse> ObtenerHorariosAsync(FiltroHorario filtros)
        {
            try
            {
                var query = HttpUtility.ParseQueryString(string.Empty);

                if (filtros.Organizacion > 0)
                    query["organizacion"] = filtros.Organizacion.ToString();

                if (!string.IsNullOrWhiteSpace(filtros.Dia))
                    query["dia"] = filtros.Dia;

                if (!string.IsNullOrWhiteSpace(filtros.ZonaId))
                    query["zonaId"] = filtros.ZonaId;

                if (filtros.Turno > 0)
                    query["turno"] = filtros.Turno.ToString();

                query["page"] = (filtros.Page - 1).ToString();
                query["size"] = filtros.Size.ToString();
                query["v"] = DateTime.Now.Ticks.ToString();

                // CORREGIDO: Prefijo correcto del gateway
                string url = $"ApiAdmi/api/horarios?{query}";

                Console.WriteLine($"[HorarioService] GET: {url}");

                var response = await _http.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"❌ Error HTTP: {response.StatusCode}");
                    return new PaginacionHorarioResponse();
                }

                var jsonString = await response.Content.ReadAsStringAsync();
                var root = JsonSerializer.Deserialize<JsonElement>(jsonString);

                var items = new List<HorarioDTO>();

                if (root.TryGetProperty("content", out var contentArray))
                {
                    foreach (var item in contentArray.EnumerateArray())
                    {
                        items.Add(MapearJsonAHorario(item));
                    }
                }

                var totalPages = root.TryGetProperty("totalPages", out var tp) ? tp.GetInt32() : 0;
                var totalElements = root.TryGetProperty("totalElements", out var te) ? te.GetInt32() : 0;
                var number = root.TryGetProperty("number", out var nb) ? nb.GetInt32() : 0;
                var last = root.TryGetProperty("last", out var l) && l.GetBoolean();
                var first = root.TryGetProperty("first", out var f) && f.GetBoolean();

                return new PaginacionHorarioResponse
                {
                    Items = items,
                    TotalPages = totalPages,
                    TotalCount = totalElements,
                    CurrentPage = number + 1,
                    HasNextPage = !last,
                    HasPreviousPage = !first
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Error en HorarioService: {ex.Message}");
                return new PaginacionHorarioResponse();
            }
        }

        public async Task<HorarioDTO?> ObtenerPorIdAsync(int id)
        {
            try
            {
                var url = $"ApiAdmi/api/horarios/{id}?v={DateTime.Now.Ticks}";
                var response = await _http.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return null;

                var jsonString = await response.Content.ReadAsStringAsync();
                var item = JsonSerializer.Deserialize<JsonElement>(jsonString);

                var horario = MapearJsonAHorario(item);

                // ---------------------------
                // AGREGAR NOMBRES EXTRA
                // ---------------------------

                // Obtener nombre de Zona
                if (!string.IsNullOrWhiteSpace(horario.ZonaId))
                {
                    var zona = await _zonaService.ObtenerZonaPorIdAsync(horario.ZonaId);
                    horario.ZonaNombre = zona?.Nombre ?? "N/A";
                }

                // Obtener nombre de Organización
                if (horario.IdOrganizacion > 0)
                {
                    var org = await _organizacionService.ObtenerPorId(horario.IdOrganizacion);
                    horario.OrganizacionNombre = org?.nombreOrganizacion ?? "N/A";
                }

                return horario;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Error obteniendo ID {id}: {ex.Message}");
                return null;
            }
        }


        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                var url = $"ApiAdmi/api/horarios/{id}";
                var response = await _http.DeleteAsync(url);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Error eliminando ID {id}: {ex.Message}");
                return false;
            }
        }

        private HorarioDTO MapearJsonAHorario(JsonElement item)
        {
            var horario = new HorarioDTO();

            if (item.TryGetProperty("id", out var pId)) horario.Id = pId.GetInt32();
            if (item.TryGetProperty("dia", out var pDia)) horario.Dia = pDia.GetString() ?? "";
            if (item.TryGetProperty("zonaId", out var pZona)) horario.ZonaId = pZona.GetString() ?? "";
            if (item.TryGetProperty("zonaNombre", out var pZonaNombre)) horario.ZonaNombre = pZonaNombre.GetString() ?? "";
            if (item.TryGetProperty("idOrganizacion", out var pOrg)) horario.IdOrganizacion = pOrg.GetInt32();
            if (item.TryGetProperty("organizacionNombre", out var pOrgNombre)) horario.OrganizacionNombre = pOrgNombre.GetString() ?? "";
            if (item.TryGetProperty("turno", out var pTurno)) horario.Turno = (byte)pTurno.GetInt32();

            if (item.TryGetProperty("horaEntrada", out var hEntrada))
                horario.HoraEntrada = ParsearHoraJava(hEntrada);

            if (item.TryGetProperty("horaSalida", out var hSalida))
                horario.HoraSalida = ParsearHoraJava(hSalida);

            return horario;
        }

        private string ParsearHoraJava(JsonElement element)
        {
            try
            {
                if (element.ValueKind == JsonValueKind.Array)
                {
                    var arr = element.EnumerateArray().ToArray();
                    if (arr.Length >= 2)
                        return $"{arr[0].GetInt32():D2}:{arr[1].GetInt32():D2}";
                }
                else if (element.ValueKind == JsonValueKind.String)
                {
                    var texto = element.GetString();
                    if (TimeSpan.TryParse(texto, out var ts))
                        return ts.ToString(@"hh\:mm");
                    return texto ?? "";
                }
            }
            catch
            {
                return "";
            }
            return "";
        }

        public async Task<HorarioDTO?> CrearHorarioAsync(HorarioGuardarDto nuevoHorario)
        {
            try
            {
                string url = "ApiAdmi/api/horarios";
                var parametros = new Dictionary<string, string>
        {
            { "horaEntrada", nuevoHorario.HoraEntrada },
            { "horaSalida", nuevoHorario.HoraSalida },
            { "dia", nuevoHorario.Dia },
            { "idOrganizacion", nuevoHorario.IdOrganizacion.ToString() },
            { "turno", nuevoHorario.Turno.ToString() },
            { "zonaId", nuevoHorario.ZonaId }
        };

                var content = new FormUrlEncodedContent(parametros);
                var response = await _http.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var item = JsonSerializer.Deserialize<JsonElement>(jsonString);
                    return MapearJsonAHorario(item);
                }
                else
                {
                    Console.WriteLine($"❌ Error al crear horario: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Error en CrearHorarioAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<HorarioDTO?> ActualizarHorarioAsync(int id, HorarioGuardarDto horario)
        {
            try
            {
                string url = $"ApiAdmi/api/horarios/{id}";
                var parametros = new Dictionary<string, string>
        {
            { "horaEntrada", horario.HoraEntrada },
            { "horaSalida", horario.HoraSalida },
            { "dia", horario.Dia },
            { "idOrganizacion", horario.IdOrganizacion.ToString() },
            { "turno", horario.Turno.ToString() },
            { "zonaId", horario.ZonaId }
        };

                var content = new FormUrlEncodedContent(parametros);
                var response = await _http.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var item = JsonSerializer.Deserialize<JsonElement>(jsonString);
                    return MapearJsonAHorario(item);
                }
                else
                {
                    Console.WriteLine($"❌ Error al actualizar horario: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Error en ActualizarHorarioAsync: {ex.Message}");
                return null;
            }
        }

    }
}