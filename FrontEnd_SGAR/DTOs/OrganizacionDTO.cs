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

        // Modelo de organización (para listas)
        public class Organizacion
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("nombreOrganizacion")]
            public string Nombre { get; set; } = string.Empty;
            
            [JsonPropertyName("telefono")]
            public string telefono { get; set; } = string.Empty;
            
            [JsonPropertyName("email")]
            public string email { get; set; } = string.Empty;
            
            [JsonPropertyName("idMunicipio")]
            public string idMunicipio { get; set; } = string.Empty;
            
            [JsonPropertyName("notificacion")]
            public string notificacion { get; set; } = string.Empty;
            
            [JsonPropertyName("idRol")]
            public int idRol { get; set; }
        }

        // Modelo de organización detallada (para ObtenerPorId y edición)
        public class OrganizacionDetalle
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }
            [JsonPropertyName("nombreOrganizacion")]
            public string nombreOrganizacion { get; set; } = string.Empty;
            [JsonPropertyName("telefono")]
            public string telefono { get; set; } = string.Empty;
            public string email { get; set; } = string.Empty;
            [JsonPropertyName("idMunicipio")]
            public string idMunicipio { get; set; } = string.Empty;
            [JsonPropertyName("notificacion")]
            public string notificacion { get; set; } = string.Empty;
            [JsonPropertyName("idRol")]
            public int idRol { get; set; }
        }

        // Modelo para crear/actualizar organización
        public class OrganizationRequest
        {
            public string nombreOrganizacion { get; set; } = "";
            public string telefono { get; set; } = "";
            public string email { get; set; } = "";
            public string password { get; set; } = "";
            public string idMunicipio { get; set; } = "";
            public string notificacion { get; set; } = "";
            public int idRol { get; set; } = 5;
        }
    }
}
