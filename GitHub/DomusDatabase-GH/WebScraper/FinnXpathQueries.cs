namespace Domus.WebScraper;
///<summary>
///Webpage Xpath queries to extract nodes
///</summary>
static public class FinnXpathQueries 
{
    public const string FINNKODE = ".//a"; 
    public const string PRICE =  ".//div/span[contains(., 'kr')]";
    public const string AREA = ".//div/span[contains(., 'm²')]";
    public const string ADDRESS = ".//span[@class='text-14 text-gray-500']";
                                    
}
