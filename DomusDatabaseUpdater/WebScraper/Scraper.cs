using System.Net.Http;
using HtmlAgilityPack;

namespace Domus.WebScraper
{
    public class Scraper
    {
        public static HtmlDocument GetHtml(string URL) 
        {
            HtmlWeb web = new();
            return web.Load(URL);
        }
        
        //Summary :
        //  Extract properties from html after XpathQueries
        //  Into class RealEstateAddData
        public static List<FinnRealEstateAddData> ExtractAddData(HtmlDocument html)
        {   
            
            var ads = html.DocumentNode.Descendants("article").Take(4);
        
            List<FinnRealEstateAddData> realEstateAddsInfo = new();
            foreach (var add in ads)
            {
                realEstateAddsInfo.Add(new FinnRealEstateAddData
                {   
                    finnkode = add.SelectSingleNode(XpathQueries.FINNKODE).GetAttributeValue("id",""),
                    price = ExtractNums(add.SelectSingleNode(XpathQueries.PRICE).InnerText),
                    sqm = ExtractNums(add.SelectSingleNode(XpathQueries.AREA).InnerText),
                    address = add.SelectSingleNode(XpathQueries.ADDRESS).InnerText

                    // Add method for extracting add publish date
                }); 
            }
            return realEstateAddsInfo;
        }

        //Summary:
        //  Extracts all numbers from a string
        //
        //To do:
        //  Add handling for floats
        public static string ExtractNums(string data)
        {
            
            if (data.Contains("-"))
            {
                data = data.Split('-')[0];
            }
            
            var numbers = "1234567890";
            var res = String.Concat(data.Where(c => numbers.Contains(c)));

            return res;
        }

    }
}