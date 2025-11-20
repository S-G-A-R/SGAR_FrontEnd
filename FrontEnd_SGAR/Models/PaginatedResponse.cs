using System.Collections.Generic;
using System.Text.Json.Serialization; // <-- ¡Nuevo using necesario!

namespace SgarApiVenta.Client.Models
{
    // Clase genérica para manejar la respuesta paginada de cualquier tipo (T)
    public class PaginatedResponse<T>
    {
        // [JsonPropertyName("nombreQueUsaLaAPI")]
        
        [JsonPropertyName("items")]
        public List<T> Items { get; set; } = new List<T>();

        [JsonPropertyName("paginaActual")]
        public int PaginaActual { get; set; }

        [JsonPropertyName("totalPaginas")]
        public int TotalPaginas { get; set; }

        [JsonPropertyName("totalRegistros")]
        public int TotalRegistros { get; set; }

        // Propiedades de ayuda (Estas son C# y no necesitan el atributo)
        public bool TienePaginaAnterior => PaginaActual > 1;
        public bool TienePaginaSiguiente => PaginaActual < TotalPaginas;
    }
}