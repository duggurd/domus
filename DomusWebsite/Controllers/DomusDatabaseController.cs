using Microsoft.AspNetCore.Mvc;

namespace domus.Controllers;

// [ApiController]
// [Route("[controller]")]
public class DomusDatabaseController : ControllerBase
{
    private readonly ILogger<DomusDatabaseController> _logger;

    private HttpClient _domusDatabaseClient = new();

    public DomusDatabaseController(ILogger<DomusDatabaseController> logger)
    {
        _logger = logger;
    }

    public string GetAll()
    {
        var DatabaseApiUrl = "https://localhost:7115/domusdatabaseapi/getall"; // from config
        _logger.LogInformation($"Api call to: {DatabaseApiUrl}");
        return _domusDatabaseClient.GetAsync(DatabaseApiUrl).Result.Content.ReadAsStringAsync().Result; // cpuple mgbs hopfeully
    }

    public string Get(int id)
    {
        var DatabaseApiUrl = $"https://localhost:7115/domusdatabaseapi/get/{id}"; // from config
        _logger.LogInformation($"Api call to: {DatabaseApiUrl}");
        return _domusDatabaseClient.GetAsync(DatabaseApiUrl).Result.Content.ReadAsStringAsync().Result;
    }

}