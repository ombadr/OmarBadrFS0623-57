using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SpedizioniEpicode.Validations;
namespace SpedizioniEpicode.Models
{
    public class TrackingSpedizioneView
    {
        [Required(ErrorMessage = "E' richiesto il Codice Fiscale o la Partita IVA.")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Il codice fiscale deve contenere esattamente 16 caratteri." )]
        [XORValidation(nameof(PartitaIVA), ErrorMessage = "Si prega di inserire solo il codice fiscale o la partita iva.")]
        public string CodiceFiscale { get; set; }
        [StringLength(11, ErrorMessage = "La partita iva deve contenere massimo 11 caratteri.")]
        public string PartitaIVA { get; set; }
        [Required(ErrorMessage = "Il campo codice spedizione è obbligatorio.")]
        [StringLength(50, ErrorMessage = "Il codice spedizione non può superare i 50 caratteri.")]
        public string CodiceSpedizione { get; set; }
        public List<AggiornamentoSpedizione> Aggiornamenti { get; set; } = new List<AggiornamentoSpedizione>();
    }

    
}