using System.Text.Json.Serialization;

namespace FrontEnd_SGAR.DTOs
{
    public class VehiculoDTO
    {
        // Modelo de respuesta paginada
        public class VehiculoResponsePaginada
        {
            [JsonPropertyName("content")]
            public List<VehiculoDetalleDto> Content { get; set; } = new();

            [JsonPropertyName("number")]
            public int Number { get; set; }

            [JsonPropertyName("size")]
            public int Size { get; set; }

            [JsonPropertyName("totalPages")]
            public int TotalPages { get; set; }

            [JsonPropertyName("totalElements")]
            public int TotalElements { get; set; }

            [JsonPropertyName("last")]
            public bool Last { get; set; }

            [JsonPropertyName("first")]
            public bool First { get; set; }

            // Propiedades de compatibilidad
            public bool HasNextPage => !Last;
            public bool HasPreviousPage => !First;
            public int PageNumber => Number;
            public int PageSize => Size;
            public int TotalCount => TotalElements;
            public List<VehiculoDetalleDto> Items => Content;
        }

        public class MarcaDto
        {
            public int Id { get; set; }
            public string Nombre { get; set; } = "";
            public string Modelo { get; set; } = "";
        }

        public class TipoVehiculoDto
        {
            public int Id { get; set; }
            public string Descripcion { get; set; } = "";
        }

        public class OperadorDto
        {
            public int Id { get; set; }
            public int IdUser { get; set; }
            public string CodigoOperador { get; set; } = "";
            public int IdOrganizacion { get; set; }
        }

        public class VehiculoDetalleDto
        {
            public int Id { get; set; }
            public string NombreMarca { get; set; } = "";
            public string ModeloMarca { get; set; } = "";
            public string Placa { get; set; } = "";
            public string Codigo { get; set; } = "";
            public string TipoVehiculoDescripcion { get; set; } = "";
            public int? IdOperador { get; set; }
            public string? Mecanico { get; set; }
            public string? Descripcion { get; set; }
            public string? Taller { get; set; }
            public int? IdFoto { get; set; }
            public string NombreOperador { get; set; } = "";
            public string ImagenBase64 { get; set; } = "";
            public byte Estado { get; set; }
            public int? IdMarca { get; set; }
            public int? IdTipoVehiculo { get; set; }
        }

        // Modelo de filtros
        public class FiltroVehiculoModel
        {
            public string? Placa { get; set; }
            public string? Codigo { get; set; }
            public string? MarcaId { get; set; }
            public string? TipoVehiculoId { get; set; }
            public string? Estado { get; set; }
            public string? Mecanico { get; set; }
        }
    }
}
