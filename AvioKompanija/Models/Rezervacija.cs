using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

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
        public void SaveToFile(string filePath)
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static Rezervacija LoadFromFile(string filePath)
        {
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Rezervacija>(json);
        }
        public static void SaveListToFile(List<Rezervacija> rezervacije, string filePath)
        {
            var json = JsonConvert.SerializeObject(rezervacije, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static List<Rezervacija> LoadListFromFile(string filePath)
        {
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Rezervacija>>(json);
        }
    }
}