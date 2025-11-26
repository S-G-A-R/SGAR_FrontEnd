using System.Text.Json.Serialization;

namespace FrontEnd_SGAR.DTOs
{

    public class PlanRequest
    {
        public int asociadoId { get; set; }
        public int tipoSuscripcionId { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public Boolean activo { get; set; } 
    }

    public class PlanResponsePaginada
    {
        [JsonPropertyName("totalItems")] public int TotalItems { get; set; }
        [JsonPropertyName("totalPages")] public int TotalPages { get; set; }
        [JsonPropertyName("pageSize")] public int PageSize { get; set; }
        [JsonPropertyName("currentPage")] public int CurrentPage { get; set; }
        [JsonPropertyName("planes")] public List<PlanDetalleDTO>? Planes { get; set; }
    }

    public class PlanDetalleDTO
    {
        [JsonPropertyName("id")] public int id { get; set; }
        [JsonPropertyName("asociadoId")] public int asociadoId { get; set; }
        [JsonPropertyName("tipoSuscripcion")] public TipoSuscripcionDTO? tipoSuscripcion { get; set; }
        [JsonPropertyName("fechaInicio")] public DateTime fechaInicio { get; set; }
        [JsonPropertyName("fechaFin")] public DateTime fechaFin { get; set; }
        [JsonPropertyName("activo")] public bool activo { get; set; }
    }

    public class PlanVerificacionResult
    {
        public bool TienePlanActivo { get; set; }
        public int DiasRestantes { get; set; }
        public PlanDetalleDTO? Plan { get; set; }
        public string? Mensaje { get; set; }
    }
}
