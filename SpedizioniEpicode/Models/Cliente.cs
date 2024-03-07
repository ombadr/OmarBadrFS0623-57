using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Win32.SafeHandles;
using SpedizioniEpicode.Validations;

namespace SpedizioniEpicode.Models
{
    public class Cliente
    {
        [Key]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "Il campo nome è obbligatorio.")]
        [StringLength(100, ErrorMessage = "Il nome non può superare i 100 caratteri.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "E' richiesto il codice fiscale o la partita iva.")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Il codice fiscale deve contenere esattamente 16 caratteri.")]
        [XORValidation(nameof(PartitaIVA), ErrorMessage = "Si prega di inserire solo il codice fiscale o la partiva iva.")]
        public string CodiceFiscale { get; set; }

        [StringLength(11, ErrorMessage = "La partita iva deve contenere massimo 11 caratteri.")]
        public string PartitaIVA { get; set; }
    }
}