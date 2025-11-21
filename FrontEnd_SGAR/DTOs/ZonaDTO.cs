using System.Text.Json.Serialization;
using static FrontEnd_SGAR.DTOs.DistritoDTO;

namespace FrontEnd_SGAR.DTOs
{
    public class ZonaDTO
    {
        //Modelo del zona
        public class Zona
        {
            [JsonPropertyName("_id")]
            public string Id { get; set; } = string.Empty;

            [JsonPropertyName("nombre")]
            public string Nombre { get; set; } = string.Empty;

            [JsonPropertyName("idDistrito")]
            public Distrito? idDistrito { get; set; }
        }

    }
}
