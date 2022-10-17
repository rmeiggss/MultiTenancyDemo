using System.ComponentModel.DataAnnotations;

namespace MultiTenancyDemo.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [EmailAddress(ErrorMessage = "El campo dene ser un correo electronico valido")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Display(Name = "Recuerdame")]
        public bool Recuerdame { get; set; }
    }
}
