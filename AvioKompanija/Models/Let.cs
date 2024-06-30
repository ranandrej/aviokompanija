using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace AvioKompanija.Models
{
    public enum Status { Aktivan,Otkazan,Zavrsen}
    public class Let
    {
        public string Id { get; set; }
        public string AvioKompanija { get; set; }
        public string PolaznaDest { get; set; }
        public string OdredisnaDest { get; set; }
        public string DatVrPolaska { get; set; }
        public string DatVrDolaska { get; set; }
        public int BrSlobodnih { get; set; }
        public int BrZauzetih { get; set; }
        public int Cena{ get; set; }
        public Status Status { get; set; }

        public Let(string id,string avioKompanija, string polaznaDest, string odredisnaDest, string datVrPolaska, string datVrDolaska, int brSlobodnih, int brZauzetih, int cena, Status status)
        {
            Id = id;
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
        public override string ToString()
        {
            return $"{Id},{AvioKompanija},{PolaznaDest},{OdredisnaDest},{DatVrPolaska},{DatVrDolaska},{BrSlobodnih},{BrZauzetih},{Cena},{(int)Status}";
        }
        public void SaveToFile(string filePath)
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static Let LoadFromFile(string filePath)
        {
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Let>(json);
        }
        public static void SaveListToFile(List<Let> let, string filePath)
        {
            var json = JsonConvert.SerializeObject(let, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static List<Let> LoadListFromFile(string filePath)
        {
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Let
                >>(json);
        }
    }
}