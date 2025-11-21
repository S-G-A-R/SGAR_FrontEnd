using System.Net.Http.Json;
using static FrontEnd_SGAR.DTOs.MunicipioDTO;

namespace FrontEnd_SGAR.Services
{
    public class MunicipioService
    {
        private readonly HttpClient _http;

        public MunicipioService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Municipio>> ObtenerMunicipiosAsync()
        {
            //Endpoint correcto para obtener todos los municipios
            var municipios = await _http.GetFromJsonAsync<List<Municipio>>("ApiNavegacion/municipalities/");
            return municipios ?? new List<Municipio>();
        }
    }
   
}

