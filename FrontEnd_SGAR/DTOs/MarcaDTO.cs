using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FrontEnd_SGAR.DTOs
{
    public class MarcaDTO
    {
        public class MarcaDetalleDto
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("nombre")]
            public string Nombre { get; set; } = "";

            [JsonPropertyName("modelo")]
            public string Modelo { get; set; } = "";

            [JsonPropertyName("yearOfFabrication")]
            public string YearOfFabrication { get; set; } = "";
        }


        public class MarcaFormModel
        {
            public int Id { get; set; }
            [Required(ErrorMessage = "El nombre es obligatorio.")]
            [RegularExpression("^[A-Za-zÁÉÍÓÚáéíóúñÑ ]+$",ErrorMessage = "El nombre solo puede contener letras.")]
            public string Nombre { get; set; } = "";

            [Required(ErrorMessage = "El modelo es obligatorio.")]
            [RegularExpression("^[A-Za-zÁÉÍÓÚáéíóúñÑ0-9 ]+$",ErrorMessage = "El modelo solo puede contener letras y números.")]
            public string Modelo { get; set; } = "";

            [Required(ErrorMessage = "El año es obligatorio.")]
            [RegularExpression("^[0-9]{4}$", ErrorMessage = "El año debe ser un número válido de 4 dígitos.")]
            [Range(0, 9999, ErrorMessage = "El año no puede ser negativo.")]
            public string YearOfFabrication { get; set; } = "";
        }


        public class MarcaResponsePaginada
        {
            [JsonPropertyName("content")]
            public List<MarcaDetalleDto> Content { get; set; } = new();

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
            public List<MarcaDetalleDto> Items => Content;
        }
    }
}
