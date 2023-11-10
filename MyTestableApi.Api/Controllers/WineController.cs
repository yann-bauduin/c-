using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace MyTestableApi.Api;


[ApiController]
[Route("api/[controller]")]
public class WinesController : ControllerBase
{
    private static List<Wine> wines = new List<Wine>
    {
        new Wine { Name = "Bordeaux", Region = "Bordeaux", Type = "Rouge" },
        new Wine { Name = "Chardonnay", Region = "Bourgogne", Type = "Blanc" },
        new Wine { Name = "Côtes du Rhône", Region = "Vallée du Rhône", Type = "Rouge" }
    };

    [HttpGet]
    public ActionResult<IEnumerable<Wine>> GetAllWines()
    {
        return wines;
    }

    [HttpGet("{name}")]
    public ActionResult<Wine> GetWineByName(string name)
    {
        var wine = wines.FirstOrDefault(w => w.Name.ToLower() == name.ToLower());
        if (wine == null)
        {
            return NotFound();
        }
        return wine;
    }

    [HttpPost]
    public ActionResult<Wine> AddWine(Wine newWine)
    {
        var existingWine = wines.FirstOrDefault(w => w.Name.ToLower() == newWine.Name.ToLower());
        if (existingWine != null)
        {
            return BadRequest("Ce vin existe déjà.");
        }

        wines.Add(newWine);
        return CreatedAtAction(nameof(GetWineByName), new { name = newWine.Name }, newWine);
    }

    [HttpPut("{name}")]
    public ActionResult UpdateWine(string name, Wine updatedWine)
    {
        var wine = wines.FirstOrDefault(w => w.Name.ToLower() == name.ToLower());
        if (wine == null)
        {
            return NotFound();
        }

        wine.Region = updatedWine.Region;
        wine.Type = updatedWine.Type;

        return NoContent();
    }

    [HttpDelete("{name}")]
    public ActionResult DeleteWine(string name)
    {
        var wine = wines.FirstOrDefault(w => w.Name.ToLower() == name.ToLower());
        if (wine == null)
        {
            return NotFound();
        }

        wines.Remove(wine);
        return NoContent();
    }
}
