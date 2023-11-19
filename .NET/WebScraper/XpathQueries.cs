namespace Domus.FinnScraper
{
    static public class XpathQueries 
    {
        public const string FINNKODE = ".//a"; 
        public const string PRICE =  "..//div[@class='col-span-2 mt-16 sm:mt-4 flex justify-between sm:block space-x-12 font-bold']/*[2]";
        public const string AREA = "..//div[@class='col-span-2 mt-16 sm:mt-4 flex justify-between sm:block space-x-12 font-bold']/*[1]";
        public const string ADDRESS = ".//span[@class='text-14 text-gray-500']";
                                        
    }
}