namespace Domus;

static public class FinnXpathQueries 
{
    public const string FINNKODE = ".//a[@id]"; 
    public const string PRICE =  ".//div/span[contains(., 'kr')]";
    public const string SQUARE_METERS = ".//div/span[contains(., 'm²')]";
    public const string ADDRESS = ".//span[@class='text-14 text-gray-500']";                                  
}