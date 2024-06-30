using AvioKompanija.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AvioKompanija.Controllers
{
    [RoutePrefix("api/admin")]
    public class AdminController : ApiController
    {
        private static List<Korisnik> users = new List<Korisnik>
        {

        };
        private List<Korisnik> LoadUsersFromFile()
        {
            users = Korisnik.LoadListFromFile("C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/korisnici.json");
            return users;
        }
        [HttpGet]
        [Route("korisnici")]
        public IEnumerable<Korisnik> Get()
        {
            users = LoadUsersFromFile();

            return users;
        }
        [HttpGet]
        [Route("searchKorisnici")]
        public IEnumerable<Korisnik> Search(string ime,string prezime)
        {


            if (ime != "")
            {
                return users.Where(u => u.Ime == ime);
            }
            if(prezime != "")
            {
                return users.Where(u => u.Prezime == prezime);
            }
            if(ime!="" && prezime != "")
            {
                return users.Where(u =>u.Ime==ime && u.Prezime == prezime);
            }
            
            return users;
        }
        [HttpGet]
        [Route("searchKompanije")]
        public IEnumerable<Models.AvioKompanija> SearchKompanije(string companyName, string companyAddress)
        {
            List<Models.AvioKompanija> kompanije = Models.AvioKompanija.LoadListFromFile("C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/aviokompanije.json");

            if (companyName != "")
            {
                return kompanije.Where(u => u.Naziv == companyName);
            }
            if (companyAddress != "")
            {
                return kompanije.Where(u => u.Adresa.Contains(companyAddress));
            }
            if (companyAddress != "" && companyName != "")
            {
                return kompanije.Where(u => u.Naziv == companyName && u.Adresa.Contains(companyAddress));
            }

            return kompanije;
        }
        [HttpGet]
        [Route("sortKorisnici")]
        public IEnumerable<Korisnik> Sort(string smer)
        {


            if (smer == "rastuce")
            {
                return users.OrderBy(u => u.Ime);
            }
            else
            {
                return users.OrderByDescending(u => u.Ime);
            }
        }
        [HttpGet]
        [Route("sortByDatumRodj")]
        public IEnumerable<Korisnik> GetUsersSortedByDateOfBirth(string smer)
        {
            if (smer == "rastuce")
            {
           
                
                return users.OrderBy(u => DateTime.ParseExact(u.DatumRodjenja, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .ToList();
            }
            else
            {
                return users.OrderByDescending(u => DateTime.ParseExact(u.DatumRodjenja, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .ToList();
            }
            

            
        }

    }
}
