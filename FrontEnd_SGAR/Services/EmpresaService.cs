using FrontEnd_SGAR.DTOs;
using System.Net.Http.Json;

namespace FrontEnd_SGAR.Services
{
    public class EmpresaService
    {
        private readonly HttpClient _http;
        public EmpresaService(HttpClient http)
        {
            _http = http;
        }

        public async Task<ResponseMessage?> CrearEmpresa(EmpresaRequest em)
        {
            var response = await _http.PostAsJsonAsync("ApiVenta/api/empresas", em);
            return await response.Content.ReadFromJsonAsync<ResponseMessage>();
        }

        public class ResponseMessage
        {
            public string? message { get; set; }
        }
    }
}
