using System.ComponentModel.DataAnnotations;

namespace FrontEnd_SGAR.Models
{
    public class CredencialesRequest
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo válido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(8, MinimumLength = 5, ErrorMessage = "La contraseña debe tener entre 5 y 8 caracteres.")]
        [RegularExpression(@"^(?!.*-\d).*", ErrorMessage = "La contraseña no puede contener números negativos.")]
        public string Password { get; set; } = string.Empty;
    }
}
