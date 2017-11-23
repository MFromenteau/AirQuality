using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using Newtonsoft.Json;

namespace AirQuality
{
    class Program
    {
        static void Main(string[] args)
        {
            bool menu = true;
            string ip;
            dynamic loc;
            dynamic quality;

            Console.WriteLine("Bonjour, Voulez-vous ? \n 1 - Entrer un adresse. \n 2 - Geolocalisation automatique.");
            while (menu)
            {
            string rl = Console.ReadLine();
            switch (rl)
                {
                    case "1":
                        Console.WriteLine("Entrez une localisation");
                        string adr = Console.ReadLine();
                        loc = GeolocalizeAdress(adr);
                        quality = GetAirQuality(loc[0].lat, loc[0].lon);
                        Console.WriteLine($" {loc[0].display_name} : \n {quality.country_description} {quality.dominant_polluant_description}");
                        menu = false;
                        break;
                    case "2":
                        ip = GetMyIp();
                        loc = GeolocalizeIp(ip);
                        quality = GetAirQuality(loc.lat, loc.lon);
                        Console.WriteLine($"{ip} at {loc.city}/{loc.country} : \n {quality.country_description} {quality.dominant_polluant_description}");
                        menu = false;
                        break;
                    default:
                        Console.WriteLine("Veuilllez choisir à nouveau.");
                        menu = true;
                        break;
                }
            }
        }

        static string GetMyIp()
        {
            WebClient webClient = new WebClient();
            return webClient.DownloadString("http://icanhazip.com").Trim();
        }

        static dynamic GeolocalizeIp(string ip)
        {
            WebClient webClient = new WebClient();
            return JsonConvert.DeserializeObject<dynamic>(webClient.DownloadString(string.Format("http://ip-api.com/json/{0}", ip).Trim()));
        }

        static dynamic GeolocalizeAdress(string adr)
        {
            WebClient webClient = new WebClient();
            string adrFormated = adr;
            adrFormated = Uri.EscapeDataString(adrFormated);
            return JsonConvert.DeserializeObject<dynamic>(webClient.DownloadString($"https://nominatim.openstreetmap.org/search.php?q={adrFormated}&format=json").Trim());
        }

        static dynamic GetAirQuality(dynamic lat, dynamic lon)
        {
            WebClient webClient = new WebClient();
            string latFormated = lat;
            latFormated = latFormated.Replace(",", ".");
            String lonFormated = lon;
            lonFormated = lonFormated.Replace(",", ".");
            return JsonConvert.DeserializeObject<dynamic>(webClient.DownloadString($"https://api.breezometer.com/baqi/?lat={latFormated}&lon={lonFormated}&key=3e3ca9627cd24faf8626cead119876ed").Trim());
        }
    }
}
