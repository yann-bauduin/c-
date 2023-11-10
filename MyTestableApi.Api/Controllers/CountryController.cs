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

    [HttpGet("test-error")]
    public ActionResult TestServerError()
    {
        try
        {
            // pour faire le test de l'erreur 500
            throw new InvalidOperationException("Erreur serveur simulée pour le test");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Une erreur interne est survenue");
            return StatusCode(500, "Une erreur interne est survenue");
        }
    }

    // ajout d'un pays
    [HttpPost]
    public ActionResult<Country> AddCountry(Country newCountry)
    {
        var existingCountry = countries.FirstOrDefault(c => c.Name.ToLower() == newCountry.Name.ToLower());
        if (existingCountry != null)
        {
            return BadRequest("Le pays existe déjà.");
        }

        countries.Add(newCountry);
        return CreatedAtAction(nameof(GetCountryByName), new { name = newCountry.Name }, newCountry);
    }

    // modification d'un pays
    [HttpPut("{name}")]
    public ActionResult UpdateCountry(string name, Country updatedCountry)
    {
        var country = countries.FirstOrDefault(c => c.Name.ToLower() == name.ToLower());
        if (country == null)
        {
            return NotFound();
        }

        country.Area = updatedCountry.Area;

        return NoContent();
    }

    // supréssion d'un pays
    [HttpDelete("{name}")]
    public ActionResult DeleteCountry(string name)
    {
        var country = countries.FirstOrDefault(c => c.Name.ToLower() == name.ToLower());
        if (country == null)
        {
            return NotFound();
        }

        countries.Remove(country);
        return NoContent();
    }

}
