using System.ComponentModel.DataAnnotations;

namespace TasksMVC.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requirido")]
        [EmailAddress(ErrorMessage = "El campo debe ser un correo electrónico válido")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
