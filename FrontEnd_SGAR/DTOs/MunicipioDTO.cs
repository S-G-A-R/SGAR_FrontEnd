using System.Text.Json.Serialization;
using static FrontEnd_SGAR.DTOs.DepartamentoDTO;

namespace FrontEnd_SGAR.DTOs
{
    public class MunicipioDTO
    {
        public class Municipio
        {
            [JsonPropertyName("_id")]
            public string Id { get; set; } = string.Empty;

            [JsonPropertyName("nombre")]
            public string Nombre { get; set; } = string.Empty;

            [JsonPropertyName("idDepartamento")]
            public Departamento? IdDepartamento { get; set; }
        }
    }
}
