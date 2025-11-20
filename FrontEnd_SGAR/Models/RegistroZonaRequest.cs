using System.ComponentModel.DataAnnotations;

namespace FrontEnd_SGAR.Models
{
    /// <summary>
    /// Modelo de validación para el registro de zona de un usuario
    /// </summary>
    public class RegistroZonaRequest
    {
        [Required(ErrorMessage = "Debe seleccionar una zona.")]
        [RegularExpression(@"^[^\d\-]+$", ErrorMessage = "La zona no puede contener números ni signos negativos.")]
        public string ZonaSeleccionada { get; set; } = string.Empty;
    }

    /// <summary>
    /// Modelo para enviar al API al registrar ciudadano
    /// </summary>
    public class RegistroCiudadanoDto
    {
        public string IdZona { get; set; } = string.Empty;
        public int IdUser { get; set; }
    }

    /// <summary>
    /// Respuesta del API al registrar ciudadano
    /// </summary>
    public class RegistroCiudadanoResponse
    {
        public int Id { get; set; }
        public string IdZona { get; set; } = string.Empty;
        public int IdUser { get; set; }
    }
}
