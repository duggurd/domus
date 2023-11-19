namespace DomusDatabase;
using Models;
using Helpers;

///<summary>
///Responsible for updating database, collecting data for updating.
///</summary>
public class DatabaseUpdater
{
    protected readonly IConfiguration _configuration;
    private Domus.WebScraper.Scraper _scraper;
    private ILogger _logger;

    public DatabaseUpdater(IConfiguration configuration, ILogger logger)
    {
        
        _configuration = configuration;
        _logger = logger;
        _scraper = new Domus.WebScraper.Scraper
        (
            (string) _configuration.GetSection("Urls").GetValue(typeof(string), "ScrapeFinn"),
            (string) _configuration.GetSection("Urls").GetValue(typeof(string), "GeodataAPISok"),
            _logger
        );
    }

    
    ///<summary>
    ///Gets needed data for, and updates DB records.
    ///</summary>
    ///
    ///To implement:
    ///    1. Make call once in a while to update database with recent articles, (1 update call every hour, 1 call every 7 days to delete all records not on finn anymore "sold")
    ///    Adds entries until an Article with a known finnkode is encountered
    ///    2. Remove records no longer on finn
    public void UpdateDatabase() 
    {   
        
        //Extracting data
        _scraper.ExtractFinnArticleInfo();
        
        //Adding data to Database
        using (DataContext db = new(_configuration))
        {   
            foreach (RealEstate realEstate in _scraper.finnRealEstateArticlesInfo)
            {
                if (db.Find<RealEstate>(realEstate.finnkodeId) != null)
                {
                    continue;
                }
                db.Add<RealEstate>(realEstate);
            }
            var entries = db.SaveChanges();
            _logger.LogInformation("Database entries created {entries}", entries);
        }
    }
} 