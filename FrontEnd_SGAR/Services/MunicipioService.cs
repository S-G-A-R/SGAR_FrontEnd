using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace FrontEnd_SGAR.Services
{
    public class MunicipioService
    {
        private readonly HttpClient _http;

        public MunicipioService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("NavigationAPI");
        }

        public async Task<List<Municipio>> ObtenerMunicipiosAsync()
        {
            //Endpoint correcto para obtener todos los municipios
            var municipios = await _http.GetFromJsonAsync<List<Municipio>>("municipalities/");
            return municipios ?? new List<Municipio>();
        }
    }

    //Modelo del municipio
    public class Municipio
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [JsonPropertyName("idDepartamento")]
        public Departamento? IdDepartamento { get; set; }
    }

    //Modelo del departamento
    public class Departamento
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;
    }
}

