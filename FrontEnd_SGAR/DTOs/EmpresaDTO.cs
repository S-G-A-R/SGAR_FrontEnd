using System.Text.Json.Serialization;

namespace FrontEnd_SGAR.DTOs
{
    public class EmpresasResponsePaginada
    {
        [JsonPropertyName("totalItems")]
        public int TotalItems { get; set; }

        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        [JsonPropertyName("hasPrevious")]
        public bool HasPrevious { get; set; }

        [JsonPropertyName("hasNext")]
        public bool HasNext { get; set; }

        [JsonPropertyName("empresas")]
        public List<EmpresaDTO> Empresas { get; set; } = new();

        [JsonPropertyName("currentPage")]
        public int CurrentPage { get; set; }

        // Propiedades de compatibilidad con la vista
        public bool HasNextPage => HasNext;
        public bool HasPreviousPage => HasPrevious;
    }

    public class EmpresaDTO
    {
        public int id { get; set; }
        public int AsociadoId { get; set; }
        public string NombreEmpresa { get; set; } = string.Empty;
    }

    public class EmpresaRequest
    {
        public int AsociadoId { get; set; }
        public string NombreEmpresa { get; set; } = string.Empty;
    }
}
