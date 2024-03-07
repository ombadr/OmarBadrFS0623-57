using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpedizioniEpicode.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using Microsoft.Ajax.Utilities;

namespace SpedizioniEpicode.Controllers
{
    public class TrackingSpedizioneController : Controller
    {
        private readonly string _connectionString;

        public TrackingSpedizioneController()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ConnessioneDBLocale"].ConnectionString + ";TrustServerCertificate=True";
        }
        // GET: TrackingSpedizione
        public ActionResult Index()
        {
            var model = new TrackingSpedizioneView();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Ricerca(TrackingSpedizioneView model)
        {
            if(ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = @"
                    SELECT a.* FROM AggiornamentoSpedizioni AS a
                    JOIN Spedizioni AS s ON a.SpedizioneId = s.SpedizioneId
                    JOIN Clienti AS c ON s.ClienteId = c.ClienteId
                    WHERE ((c.CodiceFiscale = @CodiceFiscale) OR c.PartitaIVA = @PartitaIVA)
                    AND s.NumeroIdentificativo = @NumeroIdentificativo
                    ORDER BY a.DataOraAggiornamento DESC
                    ";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CodiceFiscale", model.CodiceFiscale ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@PartitaIVA", model.PartitaIVA ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@NumeroIdentificativo", model.CodiceSpedizione);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            model.Aggiornamenti = new List<AggiornamentoSpedizione>();

                            while (reader.Read())
                            {
                                var aggiornamento = new AggiornamentoSpedizione
                                {
                                    Stato = reader["Stato"].ToString(),
                                    Luogo = reader["Luogo"].ToString(),
                                    Descrizione = reader["Descrizione"].ToString(),
                                    DataOraAggiornamento = (DateTime)reader["DataOraAggiornamento"]
                                };

                                model.Aggiornamenti.Add(aggiornamento);
                            }
                        }
                    }
                        
                }
            }
            // return same view with results
            return View("Index", model);
        }
    }
}