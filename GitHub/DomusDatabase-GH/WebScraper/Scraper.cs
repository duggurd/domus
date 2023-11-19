using System.Net.Http;
using HtmlAgilityPack;
using DomusDatabase.Models;
using Domus.Geonorge;

namespace Domus.WebScraper;

public class Scraper
{

    private readonly ILogger _logger;

    private bool debug { get; set;  }
    private string _finnURL { get; set; }
    private string _geonorgeURL { get; set; }

    private HtmlDocument html = new();

    public List<RealEstate> finnRealEstateArticlesInfo { get; set; }

    public Scraper(string finnURL, string geonorgeURL, ILogger logger)
    {

        //Add method to validate inputs
        _finnURL = finnURL;
        _geonorgeURL = geonorgeURL;
        _logger = logger;
        finnRealEstateArticlesInfo = new();
        
        //Loads html document from target webpage-url.
        HtmlWeb web = new();
        html =  web.Load(finnURL);
    }

    public void Debug(bool? debugging = null)
    {
        debug = debugging ?? (debug ? false : true) ;
        _logger.LogInformation("Debug set to: {0}", debug);
    }
    
    ///<summary>
    ///Extract properties from html with XpathQueries
    ///Into model class RealEstate
    ///</summary>
    ///
    ///<returns>
    ///List<RealEstate>
    ///</returns>
    public List<RealEstate> ExtractFinnArticleInfo() //Rewrite for dep inject
    {   

        //Must have outer loop to enum all pages. 
        //"One purpose", more abstract. Each method with own log call.
        foreach (var article in html.DocumentNode.Descendants("article").Take(8)) //Limited to 8 for testing, make into ext config 
        {
            RealEstate realEstate = new();
            
            try
            {
                //Own method to fill in "RealEstate" instance.
                //Make enumerable instead? Input list of Xpath Queries, return list/dict of nodes?
                HtmlNode finnkodeNode = article.SelectSingleNode(FinnXpathQueries.FINNKODE);
                HtmlNode priceNode = article.SelectSingleNode(FinnXpathQueries.PRICE);
                HtmlNode sqmNode = article.SelectSingleNode(FinnXpathQueries.AREA);
                HtmlNode addressNode = article.SelectSingleNode(FinnXpathQueries.ADDRESS);
                //Extract publish date
                realEstate = new()
                {                
                    finnkodeId = int.Parse((finnkodeNode).GetAttributeValue("id","")),
                    price = int.Parse(ExtractNums(priceNode.InnerText)),
                    sqm = int.Parse(ExtractNums(sqmNode.InnerText)),
                    address = addressNode.InnerText
                };
                realEstate.sqmPrice = realEstate.price / realEstate.sqm;
                //Get url of loaded article
                _logger.LogDebug("Url of article: {0}", 
                    article.SelectSingleNode(".//a").GetAttributeValue("href", ""));
                //---------------------------------------------------------------------

                //TESTING
                // _logger.LogDebug("Xpath queries {}", List<string> new(){}
                // System.Console.WriteLine("Xpath comparision, left is generated, right is premade");
                // System.Console.WriteLine($"{finnkodeNode.XPath} | {FinnXpathQueries.FINNKODE}");
                // System.Console.WriteLine($"{priceNode.XPath} | {FinnXpathQueries.PRICE}");
                // System.Console.WriteLine($"{sqmNode.XPath} | {FinnXpathQueries.AREA}");
                // System.Console.WriteLine($"{addressNode.XPath} | {FinnXpathQueries.ADDRESS}");
                //TESTING

            

                //Move code-block outside of try catch block
                var coords = GeonorgeAPI.GetGeonorgeCoords(_geonorgeURL, realEstate.address);
                if (coords.Any())   
                {   
                    realEstate.lat = coords["lat"];
                    realEstate.lon = coords["lon"];
                } 

                _logger.LogDebug("RealEstate with values {}", 
                    realEstate.GetType().GetProperties().ToList().ToString());
                //----------------------------------------------------
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                continue;
            }
            
            finally
            {   
                //Own method
                //Add to list for db update, skip if any params == 0 or null.
                if (!realEstate.GetType()
                    .GetProperties()
                    .Where(prop => prop.PropertyType == typeof(int) && 
                        prop.PropertyType == typeof(float))
                    .Any(value => value.Equals(0) || value.Equals(null)))
                {
                    finnRealEstateArticlesInfo.Add(realEstate);
                }

                _logger.LogDebug("RealEstate model added to list {0}", realEstate.finnkodeId);
                //-----------------------------------------------------------------------------
            }
        }
        return finnRealEstateArticlesInfo;
    }

    ///<summary>
    ///Extracts all numbers from a string
    ///</summary>
    ///
    ///To do:
    ///  Add float handling
    static string ExtractNums(string data)
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
