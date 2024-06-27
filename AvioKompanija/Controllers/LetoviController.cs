using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AvioKompanija.Models;
namespace AvioKompanija.Controllers
{
    public class LetoviController : ApiController
    {
        private static List<Let> letovi = new List<Let>
        {
          
        };
        private static List<Rezervacija> rezervacije = new List<Rezervacija>
        {

        };
        private static List<AvioKompanija.Models.AvioKompanija> kompanije = new List<AvioKompanija.Models.AvioKompanija>
        {

        };
        
        
        public IEnumerable<Let> Get()
        {
            string filePath = "C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/letovi.txt";
            List<Let> letoviLocal = new List<Let>();
            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                // Pretpostavimo da su podaci odvojeni zarezima
                var data = line.Split(',');

                // Kreiranje objekta Let i dodavanje u listu
                var let = new Let
                (
                    data[0],
                   data[1],
                   data[2],
                   data[3],
                   data[4],
                   data[5],
                   Int32.Parse(data[6]),
                   Int32.Parse(data[7]),
                   Int32.Parse(data[8]),
                   (Status)Int32.Parse(data[9])
                    // Dodajte ostale osobine po potrebi
                );
                
                    letoviLocal.Add(let);

                
                letovi = letoviLocal;
            }

           return letovi;
        }

        
        public IEnumerable<Let> Get(string startDest,string endDest,string startDateStr,string endDateStr,string avioKompanija)
        {
            IEnumerable<Let> filteredFlights=letovi;
            if (!string.IsNullOrEmpty(startDest) && !string.IsNullOrEmpty(endDest))
            {
                
                if (!string.IsNullOrEmpty(startDateStr) && !string.IsNullOrEmpty(endDateStr))
                {
                    if (!string.IsNullOrEmpty(avioKompanija))
                    {
                        filteredFlights = letovi.Where(l => l.AvioKompanija.Equals(avioKompanija, StringComparison.OrdinalIgnoreCase)
                        && l.DatVrPolaska.Split()[0].Equals(startDateStr, StringComparison.OrdinalIgnoreCase)
                        && l.DatVrDolaska.Split()[0].Equals(endDateStr, StringComparison.OrdinalIgnoreCase)
                        && l.PolaznaDest.Equals(startDest, StringComparison.OrdinalIgnoreCase) &&
                        l.OdredisnaDest.Equals(endDest, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                           filteredFlights = letovi.Where(l => l.DatVrPolaska.Split()[0].Equals(startDateStr, StringComparison.OrdinalIgnoreCase)
                           && l.DatVrDolaska.Split()[0].Equals(endDateStr, StringComparison.OrdinalIgnoreCase)
                           && l.PolaznaDest.Equals(startDest, StringComparison.OrdinalIgnoreCase) &&
                           l.OdredisnaDest.Equals(endDest, StringComparison.OrdinalIgnoreCase));
                    }

                   
                }
                else
                {
                    filteredFlights = letovi.Where(l => l.PolaznaDest.Equals(startDest, StringComparison.OrdinalIgnoreCase) &&
                    l.OdredisnaDest.Equals(endDest, StringComparison.OrdinalIgnoreCase));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(startDateStr) && !string.IsNullOrEmpty(endDateStr))
                {
                    if (!string.IsNullOrEmpty(avioKompanija))
                    {
                        filteredFlights = letovi.Where(l => l.AvioKompanija.Equals(avioKompanija, StringComparison.OrdinalIgnoreCase)
                        && l.DatVrPolaska.Split()[0].Equals(startDateStr, StringComparison.OrdinalIgnoreCase)
                    && l.DatVrDolaska.Split()[0].Equals(endDateStr, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        filteredFlights = letovi.Where(l => l.DatVrPolaska.Split()[0].Equals(startDateStr, StringComparison.OrdinalIgnoreCase)
                        && l.DatVrDolaska.Split()[0].Equals(endDateStr, StringComparison.OrdinalIgnoreCase));
                    }

                    
                    
                }
                else
                {
                    if (!string.IsNullOrEmpty(avioKompanija))
                    {
                        filteredFlights = letovi.Where(l =>l.AvioKompanija.Equals(avioKompanija,StringComparison.OrdinalIgnoreCase));
                    }
                }
            }

          

           
            
            return filteredFlights;

        }
        [HttpGet]
        [Route("api/letovi/sort")]
        public IEnumerable<Let>SortLetovi(string smer)
        {
            if (smer.Equals("opadajuce", System.StringComparison.OrdinalIgnoreCase))
            {
                letovi = letovi.OrderByDescending(l => l.Cena).ToList();
            }
            else
            {
                letovi = letovi.OrderBy(l => l.Cena).ToList();
            }

            return letovi;
        }
        [HttpGet]
        [Route("api/letovi/aviokompanije")]
        public IEnumerable<AvioKompanija.Models.AvioKompanija> GetAvioKompanije()
        {
            string filePath = "C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/aviokompanije.txt";
            List<AvioKompanija.Models.AvioKompanija> avioKompanije = new List<AvioKompanija.Models.AvioKompanija>();
            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                // Pretpostavimo da su podaci odvojeni zarezima
                var data = line.Split(':');

                // Kreiranje objekta Let i dodavanje u listu
                var avioKompanija = new AvioKompanija.Models.AvioKompanija
                (
                   data[0],
                   data[1],
                   data[2],
                   new List<Let>(),
                   new List<Recenzija>()
                  
                // Dodajte ostale osobine po potrebi
                );

                avioKompanije.Add(avioKompanija);
                kompanije = avioKompanije;
               
            }
            return avioKompanije;
        }
        [HttpGet]
        [Route("api/letovi/aviokompanija")]
        public AvioKompanija.Models.AvioKompanija GetKompanija(string naziv)
        {
            return kompanije.Find(k => k.Naziv == naziv);
        }
        [HttpGet]
        [Route("api/letovi/filterstatus")]
        public List<Let> FilterLet(string status)
        {
            if (status.Equals("All"))
            {
                return letovi;
            }
            return letovi.FindAll(k => k.Status == (Status)Int32.Parse(status));
        }
        [HttpPost]
        [Route("api/letovi/rezervisi")]
        public IHttpActionResult Reserve([FromBody] Rezervacija rezervacija)
        {
            

            if (rezervacija == null)
            {
                return BadRequest("Invalid reservation data.");
            }

            rezervacija.Status = StatusRez.Kreirana;
            rezervacija.Korisnik = HttpContext.Current.Session["LoggedUser"] as Korisnik;
            if (rezervacije.Count() == 0)
            {
                rezervacija.Id = "1";
            }
            else
            {
                rezervacija.Id = (Convert.ToInt32(rezervacije.Last().Id) + 1).ToString();
            }
           
            if (rezervacija.Korisnik == null)
            {
                return Unauthorized();
            }
            var let = letovi.FirstOrDefault(l=>l.Id == rezervacija.Let.Id);

            if(let == null)
            {
                return NotFound();
            }
            if (let.BrSlobodnih < rezervacija.BrojPutnika)
              {
                return BadRequest("Nema dovoljno slobodnih mesta.");
              }
            let.BrSlobodnih -= rezervacija.BrojPutnika;
            let.BrZauzetih += rezervacija.BrojPutnika;
            rezervacija.Let.BrSlobodnih -= rezervacija.BrojPutnika;
            rezervacija.Let.BrZauzetih += rezervacija.BrojPutnika;
            rezervacije.Add(rezervacija);
            var rezLine = rezervacija.ToString();
            File.AppendAllText("C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/rezervacije.txt", rezLine + '\n');
            var lines = new List<string>();

            foreach (var flight in letovi)
            {
                lines.Add(flight.ToString());
            }

            File.WriteAllText("C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/letovi.txt", string.Empty);  // Očistite fajl pre nego što upišete nove linije
            File.AppendAllLines("C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/letovi.txt", lines);
            return Ok("Uspesno kreirana rezervacija");



        }
        [HttpPost]
        [Route("api/letovi/otkazi")]
        public IHttpActionResult Cancle([FromBody] Rezervacija rezervacija)
        {

            if (rezervacija == null)
            {
                return BadRequest("Invalid reservation data.");
            }

            // Check if the reservation exists in the list
            var existingRezervacija = rezervacije.FirstOrDefault(r => r.Id == rezervacija.Id);
            if (existingRezervacija == null)
            {
                return NotFound();
            }

            // Find the flight (Let) related to the reservation
            var let = letovi.FirstOrDefault(l => l.Id == rezervacija.Let.Id);
            if (let == null)
            {
                return NotFound();
            }

            // Update flight availability
            let.BrSlobodnih += rezervacija.BrojPutnika;
            let.BrZauzetih -= rezervacija.BrojPutnika;

            // Remove the reservation from the list
            rezervacije.Remove(existingRezervacija);

            // Append reservation details to rezervacije.txt
            var lines = new List<string>();

            foreach (var rez in rezervacije)
            {
                lines.Add(rez.ToString());
            }

            File.WriteAllText("C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/rezervacije.txt", string.Empty);  // Očistite fajl pre nego što upišete nove linije
            File.AppendAllLines("C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/rezervacije.txt", lines);

            var lines2 = new List<string>();

            foreach (var flight in letovi)
            {
                lines.Add(flight.ToString());
            }

            File.WriteAllText("C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/letovi.txt", string.Empty);  // Očistite fajl pre nego što upišete nove linije
            File.AppendAllLines("C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/letovi.txt", lines2);
            return Ok("Successfully cancelled reservation.");


        }
        [HttpGet]
        [Route("api/letovi/rezervacije")]
        public IHttpActionResult GetRezByKorisnickoIme(string korisnickoIme)
        {
            LoadRezervacije();
            List<Rezervacija> rezKorisnika = new List<Rezervacija>();

            // Pronađi sve letove koji pripadaju datom korisničkom imenu
            foreach (var rezervacija in rezervacije)
            {
                if (rezervacija.Korisnik.KorisnickoIme == korisnickoIme)
                {
                    rezKorisnika.Add(rezervacija); // Dodaj letove u listu za dati korisnik
                }
            }

            if (rezKorisnika.Any())
            {
                
                return Ok(rezKorisnika);
                // Vrati sve letove pronađene za dato korisničko ime
                
            }
            else
            {
                return NotFound(); // Ako nema pronađenih letova za dato korisničko ime, vraćamo Not Found
            }
        }
        public List<Rezervacija> LoadRezervacije()
        {
            List<Rezervacija> rezUcitane = new List<Rezervacija>();
            string[] linije = File.ReadAllLines("C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/rezervacije.txt");
            foreach (var linija in linije)
            {
                string[] delovi = linija.Split(',');
                if (delovi.Length >= 22) // Ensure there are enough parts to parse
                {
                    // Assuming the format and indices are correct based on your file structure
                    Korisnik k = new Korisnik(delovi[1], delovi[2], delovi[3], delovi[4], delovi[5], delovi[6], delovi[7], (Tip)Convert.ToInt32(delovi[8]), new List<Rezervacija>());
                    Let let = new Let(delovi[9], delovi[10], delovi[11], delovi[12], delovi[13], delovi[14], Convert.ToInt32(delovi[15]), Convert.ToInt32(delovi[16]), Convert.ToInt32(delovi[17]), (Status)Convert.ToInt32(delovi[18]));
                    Rezervacija rezervacija = new Rezervacija(delovi[0], let, Convert.ToInt32(delovi[19]), Convert.ToInt32(delovi[20]), (StatusRez)Convert.ToInt32(delovi[21]));
                    rezervacija.Korisnik = k;
                    // Add the key-value pair to the dictionary
                    rezUcitane.Add(rezervacija);
                    rezervacije = rezUcitane;
                }
                
            }
            

            return rezervacije;
        }
        [HttpGet]
        [Route("api/letovi/getAll")]
        public IEnumerable<Let> GetLetovi()
        {
           
            return letovi;
        }
        [HttpGet]
        [Route("api/letovi/getAllRez")]
        public IEnumerable<Rezervacija> GetRezervacija()
        {
            

            return rezervacije;
        }

    }
  


}

