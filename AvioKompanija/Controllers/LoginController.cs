using AvioKompanija.Models;
using System;
using System.Collections.Generic;
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

        [HttpPost]
        
        public IHttpActionResult Login([FromBody] LoginModel korisnik)
            {
                if (string.IsNullOrEmpty(korisnik.KorisnickoIme) || string.IsNullOrEmpty(korisnik.Lozinka))
                {
                    return BadRequest("Invalid login request.");
                }

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
    
}


        public class LoginModel
        {
            public string KorisnickoIme { get; set; }
            public string Lozinka { get; set; }
        }

       
    }


