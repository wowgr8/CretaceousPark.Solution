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
      // updated to return the result of our Get GetAnimal route. Upon creation, the result contains a link to where that newly-created object can be found with a GET get animal request
      return CreatedAtAction(nameof(GetAnimal), new { id = animal.AnimalId }, animal);
    }

    // GET: api/Animals/5
    // THis will get a specific animal's information from the API.
    // HttpGet accepts an argument here. We include {id} in the data annotation, and restart the server.
    [HttpGet("{id}")]
    public async Task<ActionResult<Animal>> GetAnimal(int id)
    {
      var animal = await _db.Animals.FindAsync(id);

      if (animal == null)
      {
        return NotFound();
      }

      return animal;
    }
  }
}