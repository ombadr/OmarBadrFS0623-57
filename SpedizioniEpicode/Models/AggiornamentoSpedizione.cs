using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SpedizioniEpicode.Models
{
    public class AggiornamentoSpedizione
    {
        
        public int AggiornamentoSpedizioneId { get; set; }
        [Required(ErrorMessage = "Inserire lo stato della spedizione.")]
        public string Stato { get; set; }
        [StringLength(100, ErrorMessage = "Il campo luogo non può superare i 100 caratteri.")]
        [Required(ErrorMessage = "Il campo luogo è obbligatorio.")]
        public string Luogo { get; set; }
        [StringLength(1000, ErrorMessage = "Il campo descrizione non può superare i 1000 caratteri.")]
        [Required(ErrorMessage = "Il campo descrizione è obbligatorio.")]
        public string Descrizione { get; set; }
        public DateTime DataOraAggiornamento { get; set; }
        public int SpedizioneId { get; set; }
    }
}