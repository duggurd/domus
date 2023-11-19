using Microsoft.AspNetCore.Mvc;

namespace Domus;

///<summary>
///Database Api call methods.
///</summary>
public class DomusDatabaseApiController : Controller 
{
    private readonly ILogger<DomusDatabaseApiController> _logger;

    private readonly IConfiguration _configuration;

    public DomusDatabaseApiController(ILogger<DomusDatabaseApiController> logger, IConfiguration configurations)
    {
        _logger = logger;
        _configuration = configurations;
    }
  
    public IEnumerable<RealEstate>? GetAll()
    {
        using (DataContext db = new(_configuration))
        {
            var entries = db.RealEstates;

            if (entries != null && entries.Any())
            {
                _logger.LogDebug($"Response with: '{entries.Count()}' entries.");
                return entries.ToArray();
            }
            else 
            {
                _logger.LogDebug("No entries in db.");
                return null;
            }
        }
    }

    public RealEstate? Get(int id)
    {
        using(DataContext db = new(_configuration))
        {   
            var dbFind = db.Find<RealEstate>(id);
            if (dbFind != null)
            {
                _logger.LogDebug($"Response contains entry with with id: '{id}'.");
                return dbFind;
            }
            else 
            {   _logger.LogDebug($"Found no entries with id: '{id}'.");
                return null;
            } 
        }
    }
}