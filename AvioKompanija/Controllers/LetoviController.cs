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
        private static List<Recenzija> recenzije = new List<Recenzija>
        {

        };

        private static List<AvioKompanija.Models.AvioKompanija> kompanije = new List<AvioKompanija.Models.AvioKompanija>
        {

        };


        public IEnumerable<Let> Get()
        {
            letovi = Let.LoadListFromFile("C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/letovi.json");

            return letovi;
        }


        public IEnumerable<Let> Get(string startDest, string endDest, string startDateStr, string endDateStr, string avioKompanija)
        {
            IEnumerable<Let> filteredFlights = letovi;
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
                        filteredFlights = letovi.Where(l => l.AvioKompanija.Equals(avioKompanija, StringComparison.OrdinalIgnoreCase));
                    }
                }
            }





            return filteredFlights;

        }
        [HttpGet]
        [Route("api/letovi/sort")]
        public IEnumerable<Let> SortLetovi(string smer)
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

            kompanije = Models.AvioKompanija.LoadListFromFile("C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/aviokompanije.json");

            // Iteriranje kroz listu aviokompanija i čišćenje postojećih letova
            foreach (var kompanija in kompanije)
            {
                kompanija.Letovi = new List<Let>();
            }

            // Iteriranje kroz listu letova
            foreach (var let in letovi)
            {
                // Pronaći index aviokompanije koja odgovara letovom Aviokompanija polju
                var kompIndex = kompanije.FindIndex(k => k.Naziv == let.AvioKompanija);

                if (kompIndex != -1)
                {
                    // Ako aviokompanija postoji, dodajte let u njenu listu letova
                    kompanije[kompIndex].Letovi.Add(let);
                }
            }

            return kompanije;
        }
        [HttpPost]
        [Route("api/letovi/addKompanija")]
        public IHttpActionResult AddKompanija([FromBody] Models.AvioKompanija kompanija)
        {
            if (kompanija == null || kompanija.Naziv=="" || kompanija.Adresa=="" || kompanija.Informacije=="")
            {
                return BadRequest("Greska morate popuniti sva polja!");
            }
            else
            {
                kompanije.Add(kompanija);
                Models.AvioKompanija.SaveListToFile(kompanije, "C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/aviokompanije.json");
                return Ok("Uspesno dodata kompanija");
            }
        }
        [HttpPost]
        [Route("api/letovi/addLet")]
        public IHttpActionResult AddLet([FromBody] Let let)
        {
            if (let == null || string.IsNullOrWhiteSpace(let.PolaznaDest) || string.IsNullOrWhiteSpace(let.OdredisnaDest) || string.IsNullOrWhiteSpace(let.AvioKompanija))
            {
                return BadRequest("Greška: morate popuniti sva polja!");
            }

            try
            {
                let.Id = letovi.Any() ? (Convert.ToInt32(letovi.Last().Id) + 1).ToString() : "1";

                bool kompanijaPronadjena = false;
                foreach (var kompanija in kompanije)
                {
                    if (kompanija.Naziv == let.AvioKompanija)
                    {
                        kompanija.Letovi.Add(let);
                        kompanijaPronadjena = true;
                        break;
                    }
                }

                if (!kompanijaPronadjena)
                {
                    return BadRequest("Ne postoji data aviokompanija!");
                }

                letovi.Add(let);
                Models.AvioKompanija.SaveListToFile(kompanije, "C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/kompanije.json");
                Let.SaveListToFile(letovi, "C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/letovi.json");

                return Ok("Uspešno dodat let");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpPost]
        [Route("api/letovi/removeLet")]
        public IHttpActionResult RemoveLet([FromBody] Let let)
        {
            rezervacije = Rezervacija.LoadListFromFile("C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/rezervacije.json");
            if (let == null)
            {
                return BadRequest("Greška: pogrešno popunjena polja!");
            }

            var existingLet = letovi.FirstOrDefault(l => l.Id == let.Id);
            if (existingLet == null)
            {
                return BadRequest("Let ne postoji!");
            }

            // Provera da li postoji rezervacija sa statusom 'odobrena' ili 'kreirana' koja sadrži ovaj let
            var rezervacijaPostoji = rezervacije.Any(r => r.Let.Id == let.Id && (r.Status == StatusRez.Kreirana || r.Status == StatusRez.Odobrena));
            if (rezervacijaPostoji)
            {
                return BadRequest("Let ne može biti obrisan jer postoji rezervacija sa statusom 'odobrena' ili 'kreirana' koja ga sadrži.");
            }

            // Uklanjanje leta iz liste letova
            letovi.Remove(existingLet);

            // Uklanjanje leta iz liste letova svake aviokompanije
            foreach (var komp in kompanije)
            {
                var letToRemove = komp.Letovi.FirstOrDefault(l => l.Id == let.Id);
                if (letToRemove != null)
                {
                    komp.Letovi.Remove(letToRemove);
                }
            }

            // Čuvanje ažuriranih lista u fajl
            Models.AvioKompanija.SaveListToFile(kompanije, "C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/aviokompanije.json");
            Let.SaveListToFile(letovi, "C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/letovi.json");

            return Ok("Uspešno obrisan let");
        }
        [HttpPost]
        [Route("api/letovi/changeKompanija")]
        public IHttpActionResult ChangeKompanija([FromBody] Models.AvioKompanija kompanija)
        {
            if (kompanija == null || kompanija.Adresa=="" || kompanija.Informacije=="")
            {
                return BadRequest("Greska morate popuniti sva polja!");
            }
            else
            {
                int indx = kompanije.FindIndex(k => k.Naziv == kompanija.Naziv);
                if (indx == -1)
                {
                    return BadRequest("Ne postoji data kompanija!");
                }
                kompanije[indx] = kompanija;
                Models.AvioKompanija.SaveListToFile(kompanije, "C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/aviokompanije.json");
                return Ok("Uspesno izmenjena kompanija");
            }
        }
        [HttpPost]
        [Route("api/letovi/removeKompanija")]
        public IHttpActionResult RemoveKompanija([FromBody] Models.AvioKompanija kompanija)
        {
            if (kompanija == null)
            {
                return BadRequest("Greska pogresni poopunjena polja!");
            }
            else

            
            {
                var existingKompanija = kompanije.FirstOrDefault(k => k.Naziv == kompanija.Naziv);

                kompanije.Remove(existingKompanija);
                Models.AvioKompanija.SaveListToFile(kompanije, "C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/aviokompanije.json");
                return Ok("Uspesno obrisana kompanija");
            }
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
            Let.SaveListToFile(letovi, "C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/letovi.json");
            rezervacija.Let.BrSlobodnih -= rezervacija.BrojPutnika;
            rezervacija.Let.BrZauzetih += rezervacija.BrojPutnika;
            rezervacije.Add(rezervacija);
            Rezervacija.SaveListToFile(rezervacije, "C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/rezervacije.json");
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
            Rezervacija.SaveListToFile(rezervacije, "C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/rezervacije.json");
            Let.SaveListToFile(letovi, "C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/letovi.json");

            return Ok("Successfully cancelled reservation.");


        }
        [HttpGet]
        [Route("api/letovi/rezervacije")]
        public IHttpActionResult GetRezByKorisnickoIme(string korisnickoIme)
        {
            rezervacije = Rezervacija.LoadListFromFile("C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/rezervacije.json");
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
        [HttpPost]
        [Route("api/letovi/postRecenzija")]
        public IHttpActionResult Recenzija([FromBody] Recenzija recenzija)
        {
           if(recenzija.Naslov!="" && recenzija.Sadrzaj != "")
            {
                Models.AvioKompanija kompanija = kompanije.Find(k => k.Naziv == recenzija.AvioKompanija.Naziv);
                if (kompanija.Recenzije.Count() == 0)
                {
                    recenzija.Id = "1";
                }
                else
                {
                    recenzija.Id = (Convert.ToInt32(kompanija.Recenzije.Last().Id) + 1).ToString();
                }
                kompanija.Recenzije.Add(recenzija);

                Models.AvioKompanija.SaveListToFile(kompanije, "C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/aviokompanije.json");

                return Ok("Uspesno dodata recenzija i poslata adminu na proveru!");
               
            }
            return BadRequest("Sva polja moraju biti popunjena!");
        }
        [HttpPut]
        [Route("api/letovi/updateRecenzija")]
        public IHttpActionResult UpdateRecenzija([FromBody] Recenzija recenzija)
        {
            if (recenzija.Naslov != "" && recenzija.Sadrzaj != "")
            {
                Models.AvioKompanija kompanija = kompanije.Find(k => k.Naziv == recenzija.AvioKompanija.Naziv);

                int indx = kompanija.Recenzije.FindIndex(r => r.Id == recenzija.Id);
                if(indx == -1)
                {
                    return BadRequest("Greska nemoguce promeniti datu recenziju");
                }
                kompanija.Recenzije[indx]=recenzija;

                Models.AvioKompanija.SaveListToFile(kompanije, "C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/aviokompanije.json");

                return Ok("Uspesno promenjena recenzija");

            }
            return BadRequest("Sva polja moraju biti popunjena!");
        }
        [HttpGet]
        [Route("api/letovi/recenzije")]
        public List<Recenzija> GetRecenzije(string nazivKompanije)
        {
            kompanije = Models.AvioKompanija.LoadListFromFile("C:/Users/Korisnik/source/repos/AvioKompanija/AvioKompanija/App_Data/aviokompanije.json");
            if (nazivKompanije != "")
            {
                var kompanija = kompanije.Find(k => k.Naziv == nazivKompanije);
                
                
                    return kompanija.Recenzije;

                
            }
            else
            {
                return new List<Recenzija>();
            }
            
        }
       
    }

  


}

