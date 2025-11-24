using System.Text.Json.Serialization;

namespace FrontEnd_SGAR.DTOs
{
    public class PlanDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("asociadoId")]
        public int asociadoId { get; set; }
        [JsonPropertyName("tipoSuscripcionId")]
        public  int tipoSuscripcionId { get; set; }
        [JsonPropertyName("fechaInicio")]
        public DateTime fechaInicio { get; set; }
        [JsonPropertyName("fechaFin")]
        public DateTime fechaFin { get; set; }
        [JsonPropertyName("activo")]
        public Boolean activo { get; set; }

    }

    public class PlanRequest
    {
        public int asociadoId { get; set; }
        public int tipoSuscripcionId { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public Boolean activo { get; set; } 
    }
}
