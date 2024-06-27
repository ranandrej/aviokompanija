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
            private List<Korisnik> users = new List<Korisnik>
        {
            new Korisnik("username","password","andrej","andr","mail@gmail.com","12.07.2003","M",Tip.Administrator,new List<Rezervacija>()),
            new Korisnik("ranandrej","sifra123","andrej","andr","mail@gmail.com","12.07.2003","M",Tip.Putnik,new List<Rezervacija>())
        };
        private List<Korisnik> LoadUsersFromFile()
        {
            string filePath = "C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/korisnici.txt";
            var users = new List<Korisnik>();

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var data = line.Split(',');

                    var user = new Korisnik
                    (
                        data[0],
                        data[1],
                        data[2],
                        data[3],
                        data[4],
                        data[5],
                        data[6],
                        (Tip)Enum.Parse(typeof(Tip), data[7]), // assuming Tip is an enum
                        new List<Rezervacija>() // or load reservations if needed
                    );

                    users.Add(user);
                }
            }

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
            if (string.IsNullOrEmpty(korisnik.KorisnickoIme) || string.IsNullOrEmpty(korisnik.Lozinka) || string.IsNullOrEmpty(korisnik.Ime)
                || string.IsNullOrEmpty(korisnik.Prezime) || string.IsNullOrEmpty(korisnik.Email) || string.IsNullOrEmpty(korisnik.DatumRodjenja)
                || string.IsNullOrEmpty(korisnik.Pol))
            {
                return BadRequest("Greska,morate popuniti sva polja!.");
            }

            Korisnik user = new Korisnik(korisnik.KorisnickoIme, korisnik.Lozinka, korisnik.Ime, korisnik.Prezime, korisnik.Email, korisnik.DatumRodjenja
                , korisnik.Pol,Tip.Putnik, new List<Rezervacija>());

            var users = LoadUsersFromFile();
            if (users.Any(u => u.KorisnickoIme == user.KorisnickoIme))
            {
                return BadRequest("Username vec postoji.");
            }
            else
            {
                HttpContext.Current.Session["LoggedUser"] = user;
                var userLine = user.ToString();
                File.AppendAllText("C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/korisnici.txt", userLine + '\n');

            }

            // Postavljanje korisnika u sesiju

            return Ok("Login successful");
        }
        [HttpGet]
        [Route("currentuser")]
        public IHttpActionResult GetCurrentUser()
        {
            var loggedUser = HttpContext.Current.Session["LoggedUser"] as Korisnik;
            if (loggedUser == null)
            {
                return Unauthorized();
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


