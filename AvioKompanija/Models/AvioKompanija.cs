using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AvioKompanija.Models
{
    public class AvioKompanija
    {
        public string Naziv { get; set; }
        public string Adresa { get; set; }
        public string Informacije { get; set; }
        public List<Let> Letovi { get; set; }
        public List<Recenzija> Recenzije { get; set; }

        public AvioKompanija(string naziv, string adresa, string informacije, List<Let> letovi, List<Recenzija> recenzije)
        {
            Naziv = naziv;
            Adresa = adresa;
            Informacije = informacije;
            Letovi = letovi;
            Recenzije = recenzije;
        }
    }
}