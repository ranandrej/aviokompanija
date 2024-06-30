using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

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
        public override string ToString()
        {
            return $"{Naziv}:{Adresa}:{Informacije}:{Letovi}:{Recenzije}";
        }
        public void SaveToFile(string filePath)
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.AppendAllText(filePath, json + '\n');
        }

        public static AvioKompanija LoadFromFile(string filePath)
        {
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<AvioKompanija>(json);
        }
        public static void SaveListToFile(List<AvioKompanija> avioKompanije, string filePath)
        {
            var json = JsonConvert.SerializeObject(avioKompanije, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static List<AvioKompanija> LoadListFromFile(string filePath)
        {
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<AvioKompanija>>(json);
        }
    }
}