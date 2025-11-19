using System.Net.Http.Json;
using System.Web;

namespace FrontEnd_SGAR.Services
{
    public class HorarioService
    {
        private readonly HttpClient _http;

        public HorarioService(HttpClient http)
        {
            _http = http;
        }

        // =============================================================
        // 🔹 LISTAR / FILTRAR / PAGINAR
        // =============================================================
        public async Task<PaginacionHorarioResponse?> ObtenerHorariosAsync(FiltroHorario filtros)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);

            // FILTROS SEGÚN SWAGGER (pero con nombres que usa tu Razor)
            if (!string.IsNullOrWhiteSpace(filtros.Dia)) query["dia"] = filtros.Dia;
            if (!string.IsNullOrWhiteSpace(filtros.Zona)) query["zonaId"] = filtros.Zona;
            if (!string.IsNullOrWhiteSpace(filtros.Turno)) query["turno"] = filtros.Turno;

            if (!string.IsNullOrWhiteSpace(filtros.Organizacion))
                query["organizacion"] = filtros.Organizacion; // string → backend recibe int

            if (!string.IsNullOrWhiteSpace(filtros.HoraInicio)) query["inicio"] = filtros.HoraInicio;
            if (!string.IsNullOrWhiteSpace(filtros.HoraFin)) query["fin"] = filtros.HoraFin;

            // PAGINACIÓN
            query["page"] = filtros.Page.ToString();
            query["pageSize"] = filtros.PageSize.ToString();

            string url = $"api/horarios?{query}";

            return await _http.GetFromJsonAsync<PaginacionHorarioResponse>(url);
        }

        // =============================================================
        // 🔹 OBTENER POR ID
        // =============================================================
        public async Task<HorarioDTO?> ObtenerPorIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<HorarioDTO>($"api/horarios/{id}");
        }

        // =============================================================
        // 🔹 CREAR
        // =============================================================
        public async Task<bool> CrearAsync(HorarioCreateDTO horario)
        {
            var response = await _http.PostAsJsonAsync("api/horarios", horario);
            return response.IsSuccessStatusCode;
        }

        // =============================================================
        // 🔹 EDITAR
        // =============================================================
        public async Task<bool> EditarAsync(int id, HorarioCreateDTO horario)
        {
            var response = await _http.PutAsJsonAsync($"api/horarios/{id}", horario);
            return response.IsSuccessStatusCode;
        }

        // =============================================================
        // 🔹 ELIMINAR
        // =============================================================
        public async Task<bool> EliminarAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/horarios/{id}");
            return response.IsSuccessStatusCode;
        }
    }

    // =============================================================
    // 🟣 DTOs / MODELOS — NOMBRES QUE TU RAZOR SÍ USA
    // =============================================================

    // 🔸 Modelo para mostrar en tabla
    public class HorarioDTO
    {
        public int Id { get; set; }
        public string Dia { get; set; } = "";
        public string Zona { get; set; } = "";
        public string Turno { get; set; } = "";
        public string Organizacion { get; set; } = "";
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
    }

    // 🔸 Para crear o editar (lo que pide el backend)
    public class HorarioCreateDTO
    {
        public int Organizacion { get; set; }
        public string Zona { get; set; } = "";
        public string Turno { get; set; } = "";
        public string Dia { get; set; } = "";
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
    }

    // 🔸 Filtros EXACTOS QUE USA TU RAZOR
    public class FiltroHorario
    {
        public string? Dia { get; set; }
        public string? Zona { get; set; }
        public string? Turno { get; set; }

        public string? Organizacion { get; set; } // SE MANTIENE STRING PORQUE TU RAZOR LO PASA COMO STRING

        public string? HoraInicio { get; set; }
        public string? HoraFin { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    // 🔸 Respuesta paginada
    public class PaginacionHorarioResponse
    {
        public List<HorarioDTO> Items { get; set; } = new();
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }
}


//using System.Net.Http.Json;
//using System.Web;

//namespace FrontEnd_SGAR.Services
//{
//    public class HorarioService
//    {
//        private readonly HttpClient _http;

//        public HorarioService(HttpClient http)
//        {
//            _http = http;
//        }

//        // =============================================================
//        // 🔹 LISTAR / FILTRAR / PAGINAR
//        // =============================================================
//        public async Task<PaginacionHorarioResponse?> ObtenerHorariosAsync(FiltroHorario filtros)
//        {
//            var query = HttpUtility.ParseQueryString(string.Empty);

//            // FILTROS SEGÚN TU SWAGGER
//            if (!string.IsNullOrWhiteSpace(filtros.Dia)) query["dia"] = filtros.Dia;
//            if (!string.IsNullOrWhiteSpace(filtros.ZonaId)) query["zonaId"] = filtros.ZonaId;
//            if (!string.IsNullOrWhiteSpace(filtros.Turno)) query["turno"] = filtros.Turno;

//            if (filtros.Organizacion.HasValue)
//                query["organizacion"] = filtros.Organizacion.Value.ToString();

//            if (!string.IsNullOrWhiteSpace(filtros.Inicio)) query["inicio"] = filtros.Inicio;
//            if (!string.IsNullOrWhiteSpace(filtros.Fin)) query["fin"] = filtros.Fin;

//            // PAGINACIÓN
//            query["page"] = filtros.Page.ToString();
//            query["pageSize"] = filtros.PageSize.ToString();

//            string url = $"api/horarios?{query}";

//            return await _http.GetFromJsonAsync<PaginacionHorarioResponse>(url);
//        }

//        // =============================================================
//        // 🔹 OBTENER POR ID
//        // =============================================================
//        public async Task<HorarioDto?> ObtenerPorIdAsync(int id)
//        {
//            return await _http.GetFromJsonAsync<HorarioDto>($"api/horarios/{id}");
//        }

//        // =============================================================
//        // 🔹 CREAR
//        // =============================================================
//        public async Task<bool> CrearAsync(HorarioCreateDto horario)
//        {
//            var response = await _http.PostAsJsonAsync("api/horarios", horario);
//            return response.IsSuccessStatusCode;
//        }

//        // =============================================================
//        // 🔹 EDITAR
//        // =============================================================
//        public async Task<bool> EditarAsync(int id, HorarioCreateDto horario)
//        {
//            var response = await _http.PutAsJsonAsync($"api/horarios/{id}", horario);
//            return response.IsSuccessStatusCode;
//        }

//        // =============================================================
//        // 🔹 ELIMINAR
//        // =============================================================
//        public async Task<bool> EliminarAsync(int id)
//        {
//            var response = await _http.DeleteAsync($"api/horarios/{id}");
//            return response.IsSuccessStatusCode;
//        }
//    }

//    // =============================================================
//    // 🟣 DTOs / MODELOS COMPLETOS Y CORRECTOS SEGÚN TU SWAGGER
//    // =============================================================

//    // 🔸 Para mostrar datos
//    public class HorarioDto
//    {
//        public int Id { get; set; }
//        public int IdOrganizacion { get; set; }
//        public string ZonaId { get; set; } = "";
//        public string Turno { get; set; } = "";
//        public string Dia { get; set; } = "";
//        public string Inicio { get; set; } = "";  // HH:mm
//        public string Fin { get; set; } = "";     // HH:mm
//    }

//    // 🔸 Para crear o editar (tu backend pide esto)
//    public class HorarioCreateDto
//    {
//        public int IdOrganizacion { get; set; }
//        public string ZonaId { get; set; } = "";
//        public string Turno { get; set; } = "";
//        public string Dia { get; set; } = "";
//        public string Inicio { get; set; } = "";  // HH:mm
//        public string Fin { get; set; } = "";     // HH:mm
//    }

//    // 🔸 Filtros del listado (Con nombres EXACTOS del Swagger)
//    public class FiltroHorario
//    {
//        public string? Dia { get; set; }
//        public string? ZonaId { get; set; }
//        public string? Turno { get; set; }

//        public int? Organizacion { get; set; }   // integer($int32)

//        public string? Inicio { get; set; }      // HH:mm
//        public string? Fin { get; set; }         // HH:mm

//        // 🔹 Paginación
//        public int Page { get; set; } = 1;
//        public int PageSize { get; set; } = 10;
//    }

//    // 🔸 Respuesta paginada desde el backend
//    public class PaginacionHorarioResponse
//    {
//        public List<HorarioDto> Items { get; set; } = new();
//        public int TotalPages { get; set; }
//        public int TotalCount { get; set; }
//        public bool HasNextPage { get; set; }
//        public bool HasPreviousPage { get; set; }
//    }
//}
