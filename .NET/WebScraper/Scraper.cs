using System.Net.Http;
using HtmlAgilityPack;

namespace Domus.FinnScraper
{
    public class Scraper
    {
        public static HtmlDocument GetHtml(string URL) 
        {
            HtmlWeb web = new();
            return web.Load(URL);
        }
        
        ///<summary>Extract properties from html after XpathQueries</summary>
        ///<returns>List<Housinfo></returns>
        static public List<RealEstateAddInfo> ExtractHouseProperties(HtmlDocument html)
        {   
            
            var ads = html.DocumentNode.Descendants("article").Take(4);
            // var ads = html.DocumentNode.SelectNodes("//article");
            // [@class='relative overflow-hidden transition-all outline-none sf-ad-outline sf-ad-card rounded-8 mt-24 mx-16 mb-16 sm:mb-24 relative grid f-grid grid-cols-3 grid-rows-2']
        
            List<RealEstateAddInfo> realEstateAddsInfo = new();
            foreach (var add in ads)
            {
                realEstateAddsInfo.Add(new RealEstateAddInfo
                {   
                    finnkode = add.SelectSingleNode(XpathQueries.FINNKODE).GetAttributeValue("id",""),
                    price_nok = ExtractNums(add.SelectSingleNode(XpathQueries.PRICE).InnerText),
                    area_sqm = ExtractNums(add.SelectSingleNode(XpathQueries.AREA).InnerText),
                    address = add.SelectSingleNode(XpathQueries.ADDRESS).InnerText

                    // Add method for extracting add publish date
                }); 
            }
            return realEstateAddsInfo;
        }
        public static string ExtractNums(string input)
        {
            
            if (input.Contains("-"))
            {
                System.Console.WriteLine(input);
                input = input.Split('-')[0];
                System.Console.WriteLine(input);
            }
            
            var numbers = "1234567890";
            var res = String.Concat(input.Where(c => numbers.Contains(c)));

            return res;
        }

    }
}