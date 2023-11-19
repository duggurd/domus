namespace Domus.DatabaseUpdater;
public class DatabaseUpdater
{
    protected readonly IConfiguration Configuration;

    public DatabaseUpdater(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void UpdateDbQuery(List<FinnRealEstateAddData> queryData) 
    {
        
    }
} 