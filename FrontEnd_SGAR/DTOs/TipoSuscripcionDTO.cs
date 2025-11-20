using System.Text.Json.Serialization;

namespace FrontEnd_SGAR.DTOs
{
    /// <summary>
    /// Modelo de respuesta paginada para tipos de suscripción
    /// </summary>
    public class TipoSuscripcionResponsePaginada
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

        [JsonPropertyName("tiposSuscripcion")]
        public List<TipoSuscripcionDTO> TiposSuscripcion { get; set; } = new();

        [JsonPropertyName("currentPage")]
        public int CurrentPage { get; set; }

        // Propiedades de compatibilidad con la vista
        public bool HasNextPage => HasNext;
        public bool HasPreviousPage => HasPrevious;
    }

    /// <summary>
    /// Modelo de Tipo de Suscripción
    /// </summary>
    public class TipoSuscripcionDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("suscripcionNombre")]
        public string SuscripcionNombre { get; set; } = string.Empty;

        [JsonPropertyName("precio")]
        public decimal Precio { get; set; }

        [JsonPropertyName("limite")]
        public int Limite { get; set; }
    }
}
