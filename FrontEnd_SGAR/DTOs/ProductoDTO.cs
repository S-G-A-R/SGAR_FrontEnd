using System.Text.Json.Serialization;

namespace FrontEnd_SGAR.DTOs
{
    public class ProductoResponsePaginada
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

        [JsonPropertyName("productos")]
        public List<ProductoDTO> Productos { get; set; } = new();

        [JsonPropertyName("currentPage")]
        public int CurrentPage { get; set; }

        // Propiedades de compatibilidad con la vista
        public bool HasNextPage => HasNext;
        public bool HasPreviousPage => HasPrevious;
    }

    public class ProductoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int CategoriaProductoId { get; set; }
        public int EmpresaId { get; set; }

        [JsonPropertyName("fotoId")]
        public int? FotoId { get; set; }

        public string ImagenBase64 { get; set; } = string.Empty;
    }
}
