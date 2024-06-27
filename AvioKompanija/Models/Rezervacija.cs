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
        public string Id { get;  set; }

        public Rezervacija()
        {
            // Parameterless constructor for deserialization
        }
        public Rezervacija(Let let, int brojPutnika, int ukupnaCena, StatusRez status)
        {
            
            Let = let;
            BrojPutnika = brojPutnika;
            UkupnaCena = ukupnaCena;
            Status = status;
        }
        public Rezervacija(string id,Let let, int brojPutnika, int ukupnaCena, StatusRez status)
        {
            Id = id;
            Let = let;
            BrojPutnika = brojPutnika;
            UkupnaCena = ukupnaCena;
            Status = status;
        }
        public override string ToString()
        {
            return $"{Id},{Korisnik},{Let},{BrojPutnika},{UkupnaCena},{(int)Status}";
        }
    }
}