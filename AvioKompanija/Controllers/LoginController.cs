using AvioKompanija.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.SessionState;

namespace AvioKompanija.Controllers
{

    [RoutePrefix("api/login")]

    public class LoginController : ApiController, IRequiresSessionState
    {
        
            // Primer hardkodovanih korisnika. U stvarnom scenariju, koristite bazu podataka.
            private static List<Korisnik> users = new List<Korisnik>
        {
            
        };
        private List<Korisnik> LoadUsersFromFile()
        {
            users = Korisnik.LoadListFromFile("C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/korisnici.json");
            return users;
        }

        [HttpPost]
        
        public IHttpActionResult Login([FromBody] LoginModel korisnik)
            {
                if (string.IsNullOrEmpty(korisnik.KorisnickoIme) || string.IsNullOrEmpty(korisnik.Lozinka))
                {
                    return BadRequest("Invalid login request.");
                }
                var users = LoadUsersFromFile();

                Korisnik user = users.FirstOrDefault(u => u.KorisnickoIme == korisnik.KorisnickoIme && u.Lozinka == korisnik.Lozinka);
                if (user == null)
                {
                    return Unauthorized();
                }
                else
                {
                    HttpContext.Current.Session["LoggedUser"] = user;

                }

            // Postavljanje korisnika u sesiju

            return Ok("Login successful");
            }
        [HttpPost]
        [Route("register")]
        public IHttpActionResult Register([FromBody] RegisterModel korisnik)
        {
            if (korisnik != null)
            {


                if (string.IsNullOrEmpty(korisnik.KorisnickoIme) || string.IsNullOrEmpty(korisnik.Lozinka) || string.IsNullOrEmpty(korisnik.Ime)
                    || string.IsNullOrEmpty(korisnik.Prezime) || string.IsNullOrEmpty(korisnik.Email) || string.IsNullOrEmpty(korisnik.DatumRodjenja)
                    || string.IsNullOrEmpty(korisnik.Pol))
                {
                    return BadRequest("Greska,morate popuniti sva polja!.");
                }

                Korisnik user = new Korisnik(korisnik.KorisnickoIme, korisnik.Lozinka, korisnik.Ime, korisnik.Prezime, korisnik.Email, korisnik.DatumRodjenja
                    , korisnik.Pol, Tip.Putnik, new List<Rezervacija>());

                LoadUsersFromFile();
                if (users.Any(u => u.KorisnickoIme == user.KorisnickoIme))
                {
                    return BadRequest("Username vec postoji.");
                }
                else
                {
                    HttpContext.Current.Session["LoggedUser"] = user;
                    users.Add(user);
                    Korisnik.SaveListToFile(users, "C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/korisnici.json");

                }

                // Postavljanje korisnika u sesiju

                return Ok("Login successful");
            }
            return BadRequest("Morate popuniti sva polja");
        }
        [HttpPut]
        [Route("updateUser")]
        public IHttpActionResult Update([FromBody] Korisnik korisnik)
        {
            if (korisnik == null || string.IsNullOrEmpty(korisnik.KorisnickoIme) || string.IsNullOrEmpty(korisnik.Lozinka) || string.IsNullOrEmpty(korisnik.Ime)
        || string.IsNullOrEmpty(korisnik.Prezime) || string.IsNullOrEmpty(korisnik.Email) || string.IsNullOrEmpty(korisnik.DatumRodjenja)
        || string.IsNullOrEmpty(korisnik.Pol))
            {
                return BadRequest("Greska, morate popuniti sva polja!");
            }

            // Load the existing users from the file
            var users = Korisnik.LoadListFromFile("C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/korisnici.json");

            // Find the index of the user to be updated
            var userIndex = users.FindIndex(u => u.KorisnickoIme == korisnik.KorisnickoIme);

            if (userIndex == -1)
            {
                return BadRequest("Dati korisnik ne postoji.");
            }
            else
            {
                // Update the user
                users[userIndex] = korisnik;
                HttpContext.Current.Session["LoggedUser"] = korisnik;
                // Save the updated user list back to the file
                Korisnik.SaveListToFile(users, "C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/korisnici.json");
            }

            return Ok("Update successful");
        }
        [HttpGet]
        [Route("currentuser")]
        public IHttpActionResult GetCurrentUser()
        {
            var loggedUser = HttpContext.Current.Session["LoggedUser"] as Korisnik;
            if (loggedUser == null)
            {
                return Ok("Neprijavljeni korisnik");
            }

            return Ok(loggedUser);
        }
        [HttpPost]
        [Route("logout")]
        public IHttpActionResult Logout()
        {
            if (HttpContext.Current.Session["LoggedUser"] != null)
            {
                HttpContext.Current.Session.Remove("LoggedUser");
            }

            return Ok("User logged out successfully.");
        }

    }


        public class LoginModel
        {
            public string KorisnickoIme { get; set; }
            public string Lozinka { get; set; }
        }
        public class RegisterModel
        {
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Email { get; set; }
        public string DatumRodjenja { get; set; }
        public string Pol { get; set; }
    }


}


