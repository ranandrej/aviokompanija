using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AvioKompanija.Models;
namespace AvioKompanija.Controllers
{
    public class LetoviController : ApiController
    {
        private static List<Let> letovi = new List<Let>
        {
            new Let("AirSerbia","Beograd","Zagreb","21/07/2024 11:10","21/07/2024 12:00",30,10,11000,Status.Aktivan),
            new Let("AirSerbia","Beograd","Zagreb","22/07/2024 11:10","22/07/2024 12:00",10,30,10500,Status.Aktivan),
            new Let("WizzAir","London","Ibiza","29/06/2024 14:10","29/06/2024 17:00",15,10,14000,Status.Aktivan),
            new Let("AirSerbia","Beograd","Atina","21/07/2024 11:20","21/07/2024 12:30",40,20,16000,Status.Otkazan),
            new Let("RyanAir","Frankfurt","Istanbul","10/08/2024 08:10","10/08/2024 12:30",50,15,26000,Status.Aktivan)
        };
        public IEnumerable<Let> Get()
        {
            return letovi;
        }
        public IEnumerable<Let> Get(string startDest,string endDest,string startDateStr,string endDateStr,string avioKompanija)
        {
            IEnumerable<Let> filteredFlights=letovi;
            if (!string.IsNullOrEmpty(startDest) && !string.IsNullOrEmpty(endDest))
            {
                
                if (!string.IsNullOrEmpty(startDateStr) && !string.IsNullOrEmpty(endDateStr))
                {
                    if (!string.IsNullOrEmpty(avioKompanija))
                    {
                        filteredFlights = letovi.Where(l => l.AvioKompanija.Equals(avioKompanija, StringComparison.OrdinalIgnoreCase)
                        && l.DatVrPolaska.Split()[0].Equals(startDateStr, StringComparison.OrdinalIgnoreCase)
                        && l.DatVrDolaska.Split()[0].Equals(endDateStr, StringComparison.OrdinalIgnoreCase)
                        && l.PolaznaDest.Equals(startDest, StringComparison.OrdinalIgnoreCase) &&
                        l.OdredisnaDest.Equals(endDest, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                           filteredFlights = letovi.Where(l => l.DatVrPolaska.Split()[0].Equals(startDateStr, StringComparison.OrdinalIgnoreCase)
                           && l.DatVrDolaska.Split()[0].Equals(endDateStr, StringComparison.OrdinalIgnoreCase)
                           && l.PolaznaDest.Equals(startDest, StringComparison.OrdinalIgnoreCase) &&
                           l.OdredisnaDest.Equals(endDest, StringComparison.OrdinalIgnoreCase));
                    }

                   
                }
                else
                {
                    filteredFlights = letovi.Where(l => l.PolaznaDest.Equals(startDest, StringComparison.OrdinalIgnoreCase) &&
                    l.OdredisnaDest.Equals(endDest, StringComparison.OrdinalIgnoreCase));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(startDateStr) && !string.IsNullOrEmpty(endDateStr))
                {
                    if (!string.IsNullOrEmpty(avioKompanija))
                    {
                        filteredFlights = letovi.Where(l => l.AvioKompanija.Equals(avioKompanija, StringComparison.OrdinalIgnoreCase)
                        && l.DatVrPolaska.Split()[0].Equals(startDateStr, StringComparison.OrdinalIgnoreCase)
                    && l.DatVrDolaska.Split()[0].Equals(endDateStr, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        filteredFlights = letovi.Where(l => l.DatVrPolaska.Split()[0].Equals(startDateStr, StringComparison.OrdinalIgnoreCase)
                        && l.DatVrDolaska.Split()[0].Equals(endDateStr, StringComparison.OrdinalIgnoreCase));
                    }

                    
                    
                }
                else
                {
                    if (!string.IsNullOrEmpty(avioKompanija))
                    {
                        filteredFlights = letovi.Where(l =>l.AvioKompanija.Equals(avioKompanija,StringComparison.OrdinalIgnoreCase));
                    }
                }
            }

          

           
            
            return filteredFlights;

        }
        [HttpGet]
        [Route("api/letovi/sort")]
        public IEnumerable<Let>SortLetovi(string smer)
        {
            if (smer.Equals("opadajuce", System.StringComparison.OrdinalIgnoreCase))
            {
                letovi = letovi.OrderByDescending(l => l.Cena).ToList();
            }
            else
            {
                letovi = letovi.OrderBy(l => l.Cena).ToList();
            }

            return letovi;
        }
    }
}
