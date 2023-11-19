using HtmlAgilityPack;
using Domus;

namespace Domusrefactoring;

public class FinnArticleScraper  
{

    private readonly ILogger logger;
    private string targetBaseUrl { get; set; }
    private string geonorgeURL { get; set; }

    public FinnArticleScraper(string targetBaseUrl, string geonorgeURL, ILogger logger) {
        // TODO Add method to validate inputs
        this.targetBaseUrl = targetBaseUrl;
        this.geonorgeURL = geonorgeURL;
        this.logger = logger;
    }

    
    public List<RealEstate> ExtractFinnArticlesDataFromPage(int page) {
        logger.LogInformation("Scraping page: {0}", page);

    }
    private HtmlDocument LoadWebPageHtml(string targetUrl) {
        var web = new HtmlWeb();
        return web.Load(GetScrapeUrl(targetUrl));
    }



    public List<RealEstate> ExtractFinnArticleInfo(int page = 1)
    {   
        logger.LogInformation("Scraping page: {0}", page);

        var finnRealEstateArticles = new List<RealEstate>();

        //Loads html document from target webpage-url.
        HtmlWeb web = new();
        var webPageHtml =  web.Load(GetScrapeUrl(targetUrl, page));

        //Must have outer loop to enum all pages. 
        //"One purpose", more abstract. Each method with own log call.
        foreach (var article in webPageHtml.DocumentNode.Descendants("article").Take(4)) //Limited to 8 for testing, make into ext config 
        {
            RealEstate realEstate = new();
            
            try
            {
                //Get url of current article
                logger.LogDebug("Url of article: {0}", 
                    article.SelectSingleNode(".//a").GetAttributeValue("href", ""));

                HtmlNode finnkodeNode = article.SelectSingleNode(FinnXpathQueries.FINNKODE);
                HtmlNode priceNode = article.SelectSingleNode(FinnXpathQueries.PRICE);
                HtmlNode sqmNode = article.SelectSingleNode(FinnXpathQueries.AREA);
                HtmlNode addressNode = article.SelectSingleNode(FinnXpathQueries.ADDRESS);
                
                realEstate.finnkodeId = int.Parse(WebscraperUtilities.ExtractNums((finnkodeNode).GetAttributeValue("id","")));
                realEstate.price = int.Parse(WebscraperUtilities.ExtractNums(priceNode.InnerText));
                if (!int.TryParse(WebscraperUtilities.ExtractNums(priceNode.InnerText), out int x))
                {
                    logger.LogError("Node with value: '{0}' failed to parse", priceNode.InnerText);
                    continue;
                }

                else 
                {
                    realEstate.price = x;
                }
                realEstate.sqm = int.Parse(WebscraperUtilities.ExtractNums(sqmNode.InnerText));
                if (realEstate.sqm <= 2) 
                {
                    continue;
                }

                realEstate.address = WebscraperUtilities.ParseAddress(addressNode.InnerText);   
                realEstate.sqmPrice = realEstate.price / realEstate.sqm;

                var coords = GeonorgeAPI.GetGeonorgeCoords(geonorgeURL, realEstate.address);
                if (coords.Any())   
                {   
                    realEstate.lat = coords["lat"];
                    realEstate.lon = coords["lon"];
                } 


                logger.LogDebug("RealEstate with values {0}", 
                    String.Join(" | ", WebscraperUtilities.GetPropsAndValues(realEstate).Values));
            }

            catch (Exception e)
            {
                logger.LogError("{0} \n{1} \n{2}", e.Message, e.StackTrace, e.Data);
                continue;
            }
            
            finally
            {   
                //Add to list for db update, skip if any params == 0 or null.
                if (!realEstate.GetType()
                    .GetProperties()
                    .Where(prop => prop.PropertyType == typeof(int) && 
                        prop.PropertyType == typeof(float))
                    .Any(value => value.Equals(0) || value.Equals(null)))
                {
                    finnRealEstateArticles.Add(realEstate);
                }

                logger.LogDebug("RealEstate model added to list with PK: {0}", realEstate.finnkodeId);
                //-----------------------------------------------------------------------------
            }
        }
        return finnRealEstateArticles;
    }
    private string GetScrapeUrl(string url, int page = 1)
    {
        string finalUrl = url + $"&page={page}";

        return finalUrl;
    }

}
