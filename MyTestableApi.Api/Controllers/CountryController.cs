using Microsoft.AspNetCore.Mvc;

namespace MyTestableApi.Api;


[ApiController]
[Route("api/[controller]")]
public class CountriesController : ControllerBase
{
    private readonly ILogger<CountriesController> _logger;

    public CountriesController(ILogger<CountriesController> logger)
    {
        _logger = logger;
    }
    private static List<Country> countries = new List<Country>
    {
        new Country { Name = "France", Area = 551695.0 },
        new Country { Name = "Belgique", Area = 30528.0 },
        new Country { Name = "Allemagne", Area = 444953.0 },
    };

    [HttpGet]
    public ActionResult<IEnumerable<Country>> GetAllCountries()
    {
        return countries;
    }

    [HttpGet("{name}")]
    public ActionResult<Country> GetCountryByName(string name)
    {
        var country = countries.FirstOrDefault(c => c.Name.ToLower() == name.ToLower());
        if (country == null)
        {
            return NotFound();
        }
        return country;
    }
}
