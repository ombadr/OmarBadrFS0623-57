using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SpedizioniEpicode.Models
{
    public class RegisterView
    {
        [Required(ErrorMessage = "Il campo username è obbligatorio.")]
        [StringLength(50, ErrorMessage = "Il campo username non può superare i 50 caratteri.")]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Il campo password è obbligatorio.")]
        [StringLength(255, ErrorMessage = "La password non può superare i 255 caratteri.")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Le password non corrispondono.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Il campo email è obbligatorio.")]
        [StringLength(255, ErrorMessage = "L'email non può superare i 255 caratteri.")]
        [EmailAddress(ErrorMessage = "Il campo email non è un indirizzo email valido.")]
        public string Email { get; set; }
    }
}