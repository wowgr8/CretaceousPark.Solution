using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CretaceousPark.Models;

namespace CretaceousPark.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AnimalsController : ControllerBase
  {
    private readonly CretaceousParkContext _db;

    public AnimalsController(CretaceousParkContext db)
    {
      _db = db;
    }

    // GET api/animals
    [HttpGet]
    // GET route needs to return an ActionResult of type <IEnumerable<Animal>>. In our web applications, we didn't need to specify a type because we were always returning a view.
    public async Task<ActionResult<IEnumerable<Animal>>> Get()
    {
      return await _db.Animals.ToListAsync();
    }

    // POST api/animals
    [HttpPost]
    public async Task<ActionResult<Animal>> Post(Animal animal)
    {
      _db.Animals.Add(animal);
      await _db.SaveChangesAsync();

      // POST route utilizes the function CreatedAtAction. This is so that it can end up returning the Animal object to the user, as well as update the status code to 201, for "Created", rather than the default 200 OK.
      return CreatedAtAction("Post", new { id = animal.AnimalId }, animal);
    }
  }
}