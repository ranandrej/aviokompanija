using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AvioKompanija.Models
{
    public enum StatusRecenzije { Kreirana,Odobrena,Odbijena};
    public class Recenzija
    {
        public Korisnik Recezent { get; set; }
        public AvioKompanija AvioKompanija { get; set; }
        public string Naslov { get; set; }
        public string Sadrzaj { get; set; }
        public string SlikaPath { get; set; }
        public StatusRecenzije Status { get; set; }

        public Recenzija(Korisnik recezent, AvioKompanija avioKompanija, string naslov, string sadrzaj, string slikaPath, StatusRecenzije status)
        {
            Recezent = recezent;
            AvioKompanija = avioKompanija;
            Naslov = naslov;
            Sadrzaj = sadrzaj;
            SlikaPath = slikaPath;
            Status = status;
        }
    }
}