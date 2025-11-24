using System.Text.Json.Serialization;

namespace FrontEnd_SGAR.DTOs
{

    public class CategoriaProductoResponsePaginada
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

        [JsonPropertyName("categorias")]
        public List<CategoriaProductoDTO> CategoriasProducto { get; set; } = new();

        [JsonPropertyName("currentPage")]
        public int CurrentPage { get; set; }

        // Propiedades de compatibilidad con la vista
        public bool HasNextPage => HasNext;
        public bool HasPreviousPage => HasPrevious;
    }

    public class CategoriaProductoDTO
    {
        public int id { get; set; }
        public int asociadoId { get; set; }
        public string nombreCat { get; set; } = string.Empty;
    }

    public class CategoriaRequest
    {
        public int asociadoId { get; set; }
        public string nombreCat { get; set; } = string.Empty;
    }


}

