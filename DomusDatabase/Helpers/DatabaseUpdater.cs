namespace Domus;

///<summary>
///Responsible for updating database, collecting data for updating.
///</summary>
public class DatabaseUpdater : IHostedService, IDisposable
{
    protected readonly IConfiguration _configuration;
    private Scraper _scraper;
    private Timer? _timer;
    private readonly ILogger<DatabaseUpdater> _logger;

    private double _updatePeriod;
    private int _maxDbEntries;

    public DatabaseUpdater(IConfiguration configuration, ILogger<DatabaseUpdater> logger)
    {
        _configuration = configuration;
        _logger = logger;
        _scraper = new Scraper
        (
            (string) _configuration.GetSection("Scraper").GetValue(typeof(string), "ScrapeFinnUrl"),
            (string) _configuration.GetSection("Urls").GetValue(typeof(string), "GeodataAPISok"),
            _logger
        );
        _updatePeriod = (double)_configuration.GetSection("DatabaseUpdater")
            .GetValue(typeof(double), "Period");

        _maxDbEntries = (int)_configuration.GetSection("DatabaseUpdater")
            .GetValue(typeof(int), "MaxDbEntries");

        _logger.LogInformation("Updating database with a period of {0}", _updatePeriod);
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
        for (int i = 1; i <= (int)_configuration.GetSection("Scraper").GetValue(typeof(int), "MaxNumPages"); i++)
        {
            _scraper.ExtractFinnArticleInfo(i);
            
            //Adding data to Database
            using (DataContext db = new(_configuration))
            {
                int duplicates = 0;
                foreach (RealEstate realEstate in _scraper.finnRealEstateArticles)
                {
                    if (db.Find<RealEstate>(realEstate.finnkodeId) != null)
                    {
                        // stop updating if dupe found
                        _logger.LogDebug("Duplicate: Record with PK: '{0}' already in DB.", realEstate.finnkodeId);
                        duplicates ++;
                        continue;
                    }
                    db.Add<RealEstate>(realEstate);
                }
                var entries = db.SaveChanges();
                _logger.LogInformation("Database entries created: {entries} ({duplicates} duplicates).\n", entries, duplicates);
            }
        }
        TrimDatabase();
    }

    public void TrimDatabase()
    {
        using (DataContext db = new(_configuration))
        {
            if (db != null && db.RealEstates != null)
            {
                var count = db.RealEstates.Count();
                if (count > _maxDbEntries)
                {
                    db.RemoveRange(db.RealEstates.ToArray().Take(count - _maxDbEntries));
                    var entries = db.SaveChanges();
                    _logger.LogInformation("Deleted '{0}' entries from db.", entries);
                }
                else 
                {
                    _logger.LogDebug("No trimming of database, count is: {0}, max is: {1}", count, _maxDbEntries);
                }
            }
        }
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(o => {UpdateDatabase();}, 
            null, 
            TimeSpan.FromSeconds(_updatePeriod), //replace with hours when complete
            TimeSpan.FromSeconds(_updatePeriod)
        );

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
} 