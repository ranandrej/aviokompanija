﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AvioKompanija.Models
{
    public enum Tip { Putnik,Administrator}
    public class Korisnik
    {
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Email { get; set; }
        public string DatumRodjenja { get; set; }
        public string Pol { get; set; }
        public Tip Tip { get; set; }
        public List<Rezervacija> Rezervacije { get; set; }

        public Korisnik(string korisnickoIme, string lozinka, string ime, string prezime, string email, string datumRodjenja, string pol, Tip tip, List<Rezervacija> rezervacije)
        {
            KorisnickoIme = korisnickoIme;
            Lozinka = lozinka;
            Ime = ime;
            Prezime = prezime;
            Email = email;
            DatumRodjenja = datumRodjenja;
            Pol = pol;
            Tip = tip;
            Rezervacije = rezervacije;
        }
    }
}