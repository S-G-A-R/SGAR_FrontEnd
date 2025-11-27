using System.Text.Json.Serialization;

namespace FrontEnd_SGAR.DTOs
{
    public class UserResponsePaginada
    {
        [JsonPropertyName("items")]
        public List<User> Items { get; set; } = new();

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

    public class User
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [JsonPropertyName("apellido")]
        public string Apellido { get; set; } = string.Empty;

        [JsonPropertyName("telefono")]
        public string Telefono { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("dui")]
        public string DUi { get; set; } = string.Empty;

        [JsonPropertyName("foto")]
        public string foto { get; set; } = string.Empty;

        [JsonPropertyName("idRol")]
        public int idRol { get; set; }
    }

    public class userRequestRol
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("idRol")]
        public int idRol { get; set; }

    }


    public class Rol
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("nombreRol")]
        public string NombreRol { get; set; } = string.Empty;
        public List<Rol>? Item { get; internal set; }
    }
}
