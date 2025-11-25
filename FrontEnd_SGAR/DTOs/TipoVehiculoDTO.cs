using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FrontEnd_SGAR.DTOs
{
    public class TipoVehiculoDTO
    {
        public class TipoVehiculoDetalleDto
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("tipo")]
            public byte Tipo { get; set; }

            [JsonPropertyName("descripcion")]
            public string Descripcion { get; set; } = "";
        }


        public class TipoVehiculoFormModel
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "El tipo es obligatorio.")]
            [Range(0, 255, ErrorMessage = "El tipo debe ser un número válido entre 0 y 255.")]
            public byte? Tipo { get; set; }

            [Required(ErrorMessage = "La descripción es obligatoria.")]
            [RegularExpression("^[A-Za-zÁÉÍÓÚáéíóúñÑ0-9 ]+$",
                ErrorMessage = "La descripción solo puede contener letras y números.")]
            public string Descripcion { get; set; } = "";
        }


        public class TipoVehiculoResponsePaginada
        {
            [JsonPropertyName("content")]
            public List<TipoVehiculoDetalleDto> Content { get; set; } = new();

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

            // Compatibilidad
            public int PageNumber => Number;
            public int PageSize => Size;
            public int TotalCount => TotalElements;
            public List<TipoVehiculoDetalleDto> Items => Content;
        }
    }
}
