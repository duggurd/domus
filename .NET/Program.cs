using Domus.Db;
using Domus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Domus
{
    class DomusApplication
    {
        static void Main(string[] args)
        {
            // var html = FinnScraper.Scraper.GetHtml(AppConfig.Config.SCRAPE_URL);
            // var houses = FinnScraper.Scraper.ExtractHouseProperties(html);

            // var b = GeodataAPI.GeodataAPI.GetAddressInfo("oslo s");
            // var coords = GeodataAPI.GeodataAPI.ExtractCoords(b);

            // Db.PostgreSQL pg = new();
            // pg.CreateRealEstateInfo()

            //GenericUriParserOptions_chords(houses);
            //write to database

            // Models.RealEstate realEstate = new()
            // {
            //     finnkode = "abc",
            //     price_nok = 10,
            //     area_sqm = 1,
            //     address = "a",
            //     lat = 10.12f,
            //     lon = 1.014f,
            //     nok_sqm = 4
            // };
    
            // Db.Database db = new();

            // db.CreateRealEstateEntity(realEstate);

            // RealEstate? data = (RealEstate?) db.GetEntity("abc");
            // if (data != null) 
            // {
            //     System.Console.WriteLine(data.finnkode);
            // }
            


        }
    }
}