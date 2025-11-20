using System.Net.Http.Json;
using static FrontEnd_SGAR.DTOs.ZonaDTO;

namespace FrontEnd_SGAR.Services
{
    public class ZonaService
    {
        private readonly HttpClient _http;

        public ZonaService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Zona>> ObtenerZonasAsync()
        {
            //Endpoint correcto para obtener todos los municipios
            var zonas = await _http.GetFromJsonAsync<List<Zona>>("ApiNavegacion/zones/");
            return zonas ?? new List<Zona>();
        }
    }
}
