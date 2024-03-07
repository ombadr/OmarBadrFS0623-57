using SpedizioniEpicode.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using System.Data;

namespace SpedizioniEpicode.Controllers
{
    public class SpedizioniController : Controller
    {
        private readonly string _connectionString;

        public SpedizioniController()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ConnessioneDBLocale"].ConnectionString;
        }
        // GET: Spedizioni
        [Authorize]
        public ActionResult Index()
        {

            var spedizioni = new List<Spedizione>();

            try
            {
                using (SqlConnection connection =  new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "SELECT SpedizioneId, NumeroIdentificativo, DataSpedizione, " +
                        "Peso, CittàDestinataria, IndirizzoDestinatario, NominativoDestinatario, " +
                        "Costo, DataConsegnaPrevista, ClienteId FROM Spedizioni";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                var spedizione = new Spedizione
                                {
                                    SpedizioneId = (int)reader["SpedizioneId"],
                                    NumeroIdentificativo = reader["NumeroIdentificativo"].ToString(),
                                    DataSpedizione = (DateTime)reader["DataSpedizione"],
                                    Peso = (decimal)reader["Peso"],
                                    CittàDestinataria = reader["CittàDestinataria"].ToString(),
                                    IndirizzoDestinatario = reader["IndirizzoDestinatario"].ToString(),
                                    NominativoDestinatario = reader["NominativoDestinatario"].ToString(),
                                    Costo = (decimal)reader["Costo"],
                                    DataConsegnaPrevista = (DateTime)reader["DataConsegnaPrevista"],
                                    ClienteId = (int)reader["ClienteId"]
                                };

                                spedizioni.Add(spedizione);
                            }
                        }
                    }
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View(spedizioni);
        }

        public ActionResult Tracking()
        {
            return View();
        }

        [Authorize]
        public ActionResult Aggiungi()
        {
            List<Cliente> clienti = GetClientiFromDatabase();
            IEnumerable<SelectListItem> clientiList = clienti.Select(c => new SelectListItem
            {
                Value = c.ClienteId.ToString(),
                Text = c.Nome
            });

            var viewModel = new Spedizione
            {
                ClientiList = clientiList
            };

            return View(viewModel);
        }



        private List<Cliente> GetClientiFromDatabase()
        {
            List<Cliente> clienti = new List<Cliente>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT ClienteId, Nome FROM Clienti";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Cliente cliente = new Cliente
                            {
                                ClienteId = reader.GetInt32(reader.GetOrdinal("ClienteId")),
                                Nome = reader.GetString(reader.GetOrdinal("Nome"))
                            };

                            clienti.Add((cliente));
                        }
                    }
                }
            }
            return clienti;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Aggiungi([Bind(Include = "NumeroIdentificativo, DataSpedizione, Peso, CittàDestinataria, IndirizzoDestinatario, NominativoDestinatario, DataConsegnaPrevista, Costo, ClienteId ")] Spedizione spedizione)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();
                        string query = @"INSERT INTO Spedizioni 
                                (NumeroIdentificativo, DataSpedizione, Peso, CittàDestinataria, 
                                IndirizzoDestinatario, NominativoDestinatario, DataConsegnaPrevista, Costo, ClienteId)
                                VALUES (@NumeroIdentificativo, @DataSpedizione, @Peso, @CittàDestinataria,
                                @IndirizzoDestinatario, @NominativoDestinatario,@DataConsegnaPrevista, @Costo, @ClienteId)";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@NumeroIdentificativo", spedizione.NumeroIdentificativo);
                            command.Parameters.AddWithValue("@DataSpedizione", spedizione.DataSpedizione);
                            command.Parameters.AddWithValue("@Peso", spedizione.Peso);
                            command.Parameters.AddWithValue("@CittàDestinataria", spedizione.CittàDestinataria);
                            command.Parameters.AddWithValue("@IndirizzoDestinatario", spedizione.IndirizzoDestinatario);
                            command.Parameters.AddWithValue("@NominativoDestinatario", spedizione.NominativoDestinatario);
                            command.Parameters.AddWithValue("@DataConsegnaPrevista", spedizione.DataConsegnaPrevista);
                            command.Parameters.AddWithValue("@Costo", spedizione.Costo);
                            command.Parameters.AddWithValue("@ClienteId", spedizione.ClienteId);

                            command.ExecuteNonQuery();
                        }
                    }

                    return RedirectToAction("Index");

                } catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ModelState.AddModelError("", "Si è verificato un problema durante il salvataggio della spedizione: " + ex.Message);
                }
            }
            List<Cliente> clienti = GetClientiFromDatabase();
            IEnumerable<SelectListItem> clientiList = clienti.Select(c => new SelectListItem
            {
                Value = c.ClienteId.ToString(),
                Text = c.Nome
            });
            spedizione.ClientiList = clientiList;

            return View(spedizione);

          
        }


    public async Task<ActionResult> GetSpedizioniInConsegnaOggi()
        {
            List<Spedizione> spedizioni = new List<Spedizione>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Spedizioni WHERE CONVERT(date, DataConsegnaPrevista) = CONVERT(date, GETDATE())";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var spedizione = new Spedizione
                            {
                                SpedizioneId = Convert.ToInt32(reader["SpedizioneId"]),
                                NumeroIdentificativo = reader["NumeroIdentificativo"].ToString(),
                                DataSpedizione = Convert.ToDateTime(reader["DataSpedizione"]),
                                Peso = Convert.ToDecimal(reader["Peso"]),
                                CittàDestinataria = reader["CittàDestinataria"].ToString(),
                                IndirizzoDestinatario = reader["IndirizzoDestinatario"].ToString(),
                                NominativoDestinatario = reader["NominativoDestinatario"].ToString(),
                                Costo = Convert.ToDecimal(reader["Costo"]),
                                DataConsegnaPrevista = Convert.ToDateTime(reader["DataConsegnaPrevista"]),
                                ClienteId = Convert.ToInt32(reader["ClienteId"])
                            };
                            spedizioni.Add(spedizione);
                        }
                    }
                }
               

            }
            return Json(spedizioni, JsonRequestBehavior.AllowGet);
        }
        
       

    public async Task<JsonResult> GetNumeroSpedizioniInConsegna()
    {
        int numeroSpedizioniInConsegna = 0;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = @"SELECT COUNT(DISTINCT s.SpedizioneId) AS NumeroSpedizioniInConsegna
                             FROM Spedizioni s
                             INNER JOIN AggiornamentoSpedizioni a ON s.SpedizioneId = a.SpedizioneId
                             WHERE a.Stato = 'In Consegna'";
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            numeroSpedizioniInConsegna = (int)command.ExecuteScalar();
        }

        return Json(new { NumeroSpedizioniInConsegna = numeroSpedizioniInConsegna }, JsonRequestBehavior.AllowGet);
    }


        public async Task<JsonResult> GetNumeroSpedizioniPerCittaDestinataria()
        {
            Dictionary<string, int> numeroSpedizioniPerCitta = new Dictionary<string, int>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT CittàDestinataria, COUNT(*) AS NumeroSpedizioni
                         FROM Spedizioni
                         GROUP BY CittàDestinataria";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string cittaDestinataria = reader["CittàDestinataria"].ToString();
                    int numeroSpedizioni = Convert.ToInt32(reader["NumeroSpedizioni"]);
                    numeroSpedizioniPerCitta.Add(cittaDestinataria, numeroSpedizioni);
                }
            }

            return Json(numeroSpedizioniPerCitta, JsonRequestBehavior.AllowGet);
        }

    }

} 


