using System.Text.Json.Serialization;

namespace FrontEnd_SGAR.DTOs
{
    public class OrganizacionDTO
    {
        // Modelo de respuesta paginada
        public class OrganizacionResponsePaginada
        {
            [JsonPropertyName("items")]
            public List<Organizacion> Items { get; set; } = new();

            [JsonPropertyName("pageNumber")]
            public int PageNumber { get; set; }

            [JsonPropertyName("pageSize")]
            public int PageSize { get; set; }

            [JsonPropertyName("totalPages")]
            public int TotalPages { get; set; }

            [JsonPropertyName("totalCount")]
            public int TotalCount { get; set; }

            [JsonPropertyName("hasNextPage")]
            public bool HasNextPage { get; set; }

            [JsonPropertyName("hasPreviousPage")]
            public bool HasPreviousPage { get; set; }
        }

        // Modelo de organización
        public class Organizacion
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("nombreOrganizacion")]
            public string Nombre { get; set; } = string.Empty;
        }
    }
}
