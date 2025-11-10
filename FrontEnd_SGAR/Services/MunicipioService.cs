using System.Net.Http.Json;

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
            // ✅ Endpoint correcto para obtener todos los municipios
            var municipios = await _http.GetFromJsonAsync<List<Municipio>>("/municipalities/");
            return municipios ?? new List<Municipio>();
        }
    }

    public class Municipio
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public Departamento? IdDepartamento { get; set; }
    }

    public class Departamento
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
    }
}