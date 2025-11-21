using System.Text.Json.Serialization;

namespace FrontEnd_SGAR.DTOs
{
    public class CiudadanoDTO
    {
        public class Ciudadano
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("idZona")]
            public string IdZona { get; set; } = string.Empty;

            [JsonPropertyName("idUser")]
            public int IdUser { get; set; }
        }
    }
}
