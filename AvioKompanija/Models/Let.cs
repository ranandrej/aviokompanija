﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AvioKompanija.Models
{
    public enum Status { Aktivan,Otkazan,Zavrsen}
    public class Let
    {
        public string AvioKompanija { get; set; }
        public string PolaznaDest { get; set; }
        public string OdredisnaDest { get; set; }
        public string DatVrPolaska { get; set; }
        public string DatVrDolaska { get; set; }
        public int BrSlobodnih { get; set; }
        public int BrZauzetih { get; set; }
        public int Cena{ get; set; }
        public Status Status { get; set; }

        public Let(string avioKompanija, string polaznaDest, string odredisnaDest, string datVrPolaska, string datVrDolaska, int brSlobodnih, int brZauzetih, int cena, Status status)
        {
            AvioKompanija = avioKompanija;
            PolaznaDest = polaznaDest;
            OdredisnaDest = odredisnaDest;
            DatVrPolaska = datVrPolaska;
            DatVrDolaska = datVrDolaska;
            BrSlobodnih = brSlobodnih;
            BrZauzetih = brZauzetih;
            Cena = cena;
            Status = status;
        }
    }
}