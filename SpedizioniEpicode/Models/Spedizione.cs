using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpedizioniEpicode.Models
{
    public class Spedizione
    {
        public int SpedizioneId { get; set; }
        [Required(ErrorMessage = "Il campo numero identificativo è obbligatorio.")]
        [StringLength(50, ErrorMessage = "Il numero identificativo non può superare i 50 caratteri.")]
        public string NumeroIdentificativo { get; set; }
        [Required(ErrorMessage = "Il campo data spedizione è obbligatorio.")]
        [DataType(DataType.Date, ErrorMessage = "Il valore inserito non è una data valida.")]

        public DateTime DataSpedizione { get; set; }
        [Required(ErrorMessage = "Il campo peso è obbligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Il Peso deve essere maggiore di zero.")]
        public decimal Peso { get; set; }

        [Required(ErrorMessage = "Il campo città destinataria è obbligatorio.")]
        [StringLength(50, ErrorMessage = "La città destinataria non può superare i 50 caratteri.")]
        public string CittàDestinataria { get; set; }

        [Required(ErrorMessage = "Il campo indirizzo destinatario è obbligatorio.")]
        [StringLength(255, ErrorMessage = "L'indirizzo destinatario non può superare i 255 caratteri.")]
        public string IndirizzoDestinatario { get; set; }

        [Required(ErrorMessage = "Il campo nominativo destinatario è obbligatorio.")]
        [StringLength(100, ErrorMessage = "Il nominativo destinatario non può superare i 100 caratteri.")]
        public string NominativoDestinatario { get; set; }

        [Required(ErrorMessage = "Il campo costo è obbligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Il Costo deve essere maggiore di zero.")]
        [DataType(DataType.Currency, ErrorMessage = "Il valore inserito non è una valuta valida.")]
        public decimal Costo { get; set; }


        [Required(ErrorMessage = "Il campo data consegna prevista è obbligatorio.")]
        [DataType(DataType.Date, ErrorMessage = "Il valore inserito non è una data valida.")]
        public DateTime DataConsegnaPrevista { get; set; }

        [Required(ErrorMessage = "E' obbligatorio selezionare il cliente")]
        public int ClienteId { get; set; }


        public IEnumerable<SelectListItem> ClientiList { get; set; }
    }
}