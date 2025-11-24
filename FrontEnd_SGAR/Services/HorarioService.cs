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

        public async Task<PaginacionHorarioResponse?> ObtenerHorariosAsync(FiltroHorario filtros)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);

            if (!string.IsNullOrWhiteSpace(filtros.Dia)) query["dia"] = filtros.Dia;
            if (!string.IsNullOrWhiteSpace(filtros.Zona)) query["zonaId"] = filtros.Zona;
            if (!string.IsNullOrWhiteSpace(filtros.Turno)) query["turno"] = filtros.Turno;

            if (!string.IsNullOrWhiteSpace(filtros.Organizacion))
                query["organizacion"] = filtros.Organizacion; // string → backend recibe int

            if (!string.IsNullOrWhiteSpace(filtros.HoraInicio)) query["inicio"] = filtros.HoraInicio;
            if (!string.IsNullOrWhiteSpace(filtros.HoraFin)) query["fin"] = filtros.HoraFin;

            query["page"] = filtros.Page.ToString();
            query["pageSize"] = filtros.PageSize.ToString();

            string url = $"api/horarios?{query}";

            return await _http.GetFromJsonAsync<PaginacionHorarioResponse>(url);
        }

        public async Task<HorarioDTO?> ObtenerPorIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<HorarioDTO>($"api/horarios/{id}");
        }


        public async Task<bool> CrearAsync(HorarioCreateDTO horario)
        {
            var response = await _http.PostAsJsonAsync("api/horarios", horario);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EditarAsync(int id, HorarioCreateDTO horario)
        {
            var response = await _http.PutAsJsonAsync($"api/horarios/{id}", horario);
            return response.IsSuccessStatusCode;
        }


        public async Task<bool> EliminarAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/horarios/{id}");
            return response.IsSuccessStatusCode;
        }
    }

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

    public class HorarioCreateDTO
    {
        public int Organizacion { get; set; }
        public string Zona { get; set; } = "";
        public string Turno { get; set; } = "";
        public string Dia { get; set; } = "";
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
    }

    public class FiltroHorario
    {
        public string? Dia { get; set; }
        public string? Zona { get; set; }
        public string? Turno { get; set; }

        public string? Organizacion { get; set; } 

        public string? HoraInicio { get; set; }
        public string? HoraFin { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class PaginacionHorarioResponse
    {
        public List<HorarioDTO> Items { get; set; } = new();
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }
}


