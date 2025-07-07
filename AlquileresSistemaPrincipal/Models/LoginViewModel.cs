using System.ComponentModel.DataAnnotations;

namespace AlquileresSistemaPrincipal.Models
{
    // Models/LoginViewModel.cs
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Correo requerido")]
        [EmailAddress]
        public required string Correo { get; set; }

        [Required(ErrorMessage = "Contraseña requerida")]
        public  required string Contraseña { get; set; }
    }

}
