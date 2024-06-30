using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace AvioKompanija.Models
{
    public enum StatusRecenzije { Kreirana,Odobrena,Odbijena};
    public class Recenzija
    {
        public string Id { get; set; }
        public Korisnik Recezent { get; set; }
        public AvioKompanija AvioKompanija { get; set; }
        public string Naslov { get; set; }
        public int Ocena { get; set; }
        public string Sadrzaj { get; set; }
        public string SlikaPath { get; set; }
        public StatusRecenzije Status { get; set; }

        public Recenzija(Korisnik recezent, AvioKompanija avioKompanija, string naslov,int ocena, string sadrzaj, string slikaPath, StatusRecenzije status)
        {
            Recezent = recezent;
            AvioKompanija = avioKompanija;
            Naslov = naslov;
            Ocena = ocena;
            Sadrzaj = sadrzaj;
            SlikaPath = slikaPath;
            Status = status;
        }
        public override string ToString()
        {
            return $"{Recezent},{AvioKompanija},{Naslov},{Ocena},{Sadrzaj},{SlikaPath},{(int)Status}";
        }
        public void SaveToFile(string filePath)
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static Recenzija LoadFromFile(string filePath)
        {
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Recenzija>(json);
        }
    }
}