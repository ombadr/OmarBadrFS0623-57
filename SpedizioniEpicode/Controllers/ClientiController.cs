using SpedizioniEpicode.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SpedizioniEpicode.Controllers
{
    public class ClientiController : Controller
    {
        private readonly string _connectionString;

        public ClientiController()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ConnessioneDBLocale"].ConnectionString;
        }
        // GET: Clienti
        [Authorize]
        public ActionResult Index()
        {
            var clienti = new List<Cliente>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "SELECT ClienteId, Nome, CodiceFiscale, PartitaIVA FROM Clienti";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var cliente = new Cliente
                                {
                                    ClienteId = (int)reader["ClienteId"],
                                    Nome = reader["Nome"].ToString(),
                                    CodiceFiscale = reader["CodiceFiscale"].ToString(),
                                    PartitaIVA = reader["PartitaIVA"] as string,
                                };

                                clienti.Add(cliente);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View(clienti);
        }

        public ActionResult Aggiungi()
        {
            
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Aggiungi([Bind(Include = "Nome, CodiceFiscale, PartitaIVA")] Cliente cliente)
        {

            bool codFiscOrPIVA = string.IsNullOrEmpty(cliente.CodiceFiscale) ^ string.IsNullOrEmpty(cliente.PartitaIVA);

            if (!codFiscOrPIVA)
            {
                ModelState.AddModelError("", "Inserire il Codice Fiscale o la Partita IVA, ma non entrambi.");
            }
            if (ModelState.IsValid && codFiscOrPIVA)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();
                        string query = "INSERT INTO Clienti (Nome, CodiceFiscale, PartitaIVA) VALUES (@Nome, @CodiceFiscale, @PartitaIVA)";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Nome", cliente.Nome);
                            command.Parameters.AddWithValue("@CodiceFiscale", cliente.CodiceFiscale ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@PartitaIVA", cliente.PartitaIVA ?? (object)DBNull.Value);

                            command.ExecuteNonQuery();
                        }

                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                    ModelState.AddModelError("", "Impossibile salvare i dati. Riprova, e se il problema persiste, contatta l'amministratore del sistema.");
                }
            }

            return View(cliente);
        }
    }
}