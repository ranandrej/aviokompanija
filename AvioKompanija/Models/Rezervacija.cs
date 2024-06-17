using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AvioKompanija.Models
{
    public enum StatusRez { Kreirana, Odobrena, Otkazana,Zavrsena }

    public class Rezervacija
    {
        public Korisnik Korisnik { get; set; }
        public Let Let { get; set; }
        public int BrojPutnika { get; set; }
        public int UkupnaCena { get; set; }
        public StatusRez Status { get; set; }

        public Rezervacija(Korisnik korisnik,Let let, int brojPutnika, int ukupnaCena, StatusRez status)
        {
            Korisnik = korisnik;
            Let = let;
            BrojPutnika = brojPutnika;
            UkupnaCena = ukupnaCena;
            Status = status;
        }
    }
}