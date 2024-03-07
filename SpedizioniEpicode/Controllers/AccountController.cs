using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.SqlClient;
using System.Configuration;
using SpedizioniEpicode.Models;

namespace SpedizioniEpicode.Controllers
{
    public class AccountController : Controller
    {

        private readonly string _connectionString;

        public AccountController()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ConnessioneDBLocale"].ConnectionString;
        }

        private bool AuthenticateUser(string username, string password)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "SELECT Password FROM Utenti WHERE Username = @Username";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                string storePasswordHash = reader["Password"].ToString();
                                return BCrypt.Net.BCrypt.Verify(password, storePasswordHash);
                            }
                        }
                    }
                }

            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Username, Password")] LoginView loginModel)
        {
            if (ModelState.IsValid)
            {
                bool isValidUser = AuthenticateUser(loginModel.Username, loginModel.Password);

                if (isValidUser)
                {
                    FormsAuthentication.SetAuthCookie(loginModel.Username, false);
                    return RedirectToAction("Index", "Spedizioni");
                }
                else
                {
                    ModelState.AddModelError("Username", "Username o password non validi.");
                    ModelState.AddModelError("Password", "Username o password non validi.");
                    return View();
                }
            }

            return View();
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult Register()
        {

            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Username, Password, Email")] RegisterView model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();
                        string query = "INSERT INTO Utenti(Username, Password, Email, RuoloId) VALUES (@Username, @Password, @Email, @RuoloId)";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Username", model.Username);
                            command.Parameters.AddWithValue("@Password",BCrypt.Net.BCrypt.HashPassword(model.Password));
                            command.Parameters.AddWithValue("@Email", model.Email);
                            command.Parameters.AddWithValue("@RuoloId", 2);
                            command.ExecuteNonQuery();
                        }

                        return RedirectToAction("Index", "Spedizioni");
                    }

                } catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ModelState.AddModelError("", "Si è verificato un errore durante la registrazione.");
                }
            }

            return View(model);
        }
    }
}