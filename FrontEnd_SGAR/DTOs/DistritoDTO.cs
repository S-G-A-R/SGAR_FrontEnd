using System.Text.Json.Serialization;

namespace FrontEnd_SGAR.DTOs
{
    public class DistritoDTO
    {
        public class Distrito
        {
            [JsonPropertyName("_id")]
            public string Id { get; set; } = string.Empty;

            [JsonPropertyName("nombre")]
            public string Nombre { get; set; } = string.Empty;
        }
    }
}
