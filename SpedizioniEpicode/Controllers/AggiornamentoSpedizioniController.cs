using Microsoft.Data.SqlClient;
using SpedizioniEpicode.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;

namespace SpedizioniEpicode.Controllers
{
    public class AggiornamentoSpedizioniController : Controller
    {
        private readonly string _connectionString;

        public AggiornamentoSpedizioniController()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ConnessioneDBLocale"].ConnectionString + ";TrustServerCertificate=True";
        }

        // GET: AggiornamentoSpedizioni
        public ActionResult Index(int id)
        {
            var spedizioneId = id;
            var aggiornamenti = new List<AggiornamentoSpedizione>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "SELECT AggiornamentoSpedizioniId, Stato, Luogo, Descrizione, DataOraAggiornamento, SpedizioneId FROM AggiornamentoSpedizioni WHERE SpedizioneId = @SpedizioneId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SpedizioneId", spedizioneId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                var aggiornamento = new AggiornamentoSpedizione
                                {
                                    AggiornamentoSpedizioneId = (int)reader["AggiornamentoSpedizioniId"],
                                    Stato = reader["Stato"].ToString(),
                                    Luogo = reader["Luogo"].ToString(),
                                    Descrizione = reader["Descrizione"].ToString(),
                                    DataOraAggiornamento = (DateTime)reader["DataOraAggiornamento"],
                                    SpedizioneId = (int)reader["SpedizioneId"]

                                };

                                aggiornamenti.Add(aggiornamento);
                            }
                        }
                    }

                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return View(aggiornamenti);
        }

        // form per aggiungere aggiornamento
        public ActionResult Aggiungi(int id)
        {
            var spedizioneId = id;
            var model = new AggiornamentoSpedizione { SpedizioneId = spedizioneId };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Aggiungi([Bind(Include = "Stato, Luogo, Descrizione, SpedizioneId")] AggiornamentoSpedizione aggiornamento)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();
                        string query = @"INSERT INTO AggiornamentoSpedizioni (Stato, Luogo, 
                        Descrizione, DataOraAggiornamento,SpedizioneId)
                        VALUES (@Stato, @Luogo, @Descrizione,@DataOraAggiornamento, @SpedizioneId)
                        ";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Stato", aggiornamento.Stato);
                            command.Parameters.AddWithValue("@Luogo", aggiornamento.Luogo);
                            command.Parameters.AddWithValue("@Descrizione", aggiornamento.Descrizione);
                            command.Parameters.AddWithValue("@DataOraAggiornamento", DateTime.Now);
                            command.Parameters.AddWithValue("@SpedizioneId", aggiornamento.SpedizioneId);

                            command.ExecuteNonQuery();
                        }
                    }

                    return RedirectToAction("Index", "Spedizioni");

                } catch (Exception ex)
                { Console.WriteLine(ex.Message); }
            }
            return View(aggiornamento);
        }

       
    }


}