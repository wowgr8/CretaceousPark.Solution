using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CretaceousPark.Models;
// allows use of EntityState
using Microsoft.EntityFrameworkCore;

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

    // PUT: api/Animals/5
    // PUT is like POST in that it makes a change to the server. However, PUT changes existing information while POST creates new information. 

    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // [HttpPut] annotation specifies that we'll determine which animal will be updated based on the id parameter in the URL.
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Animal animal)
    {
      if (id != animal.AnimalId)
      {
        return BadRequest();
      }

      _db.Entry(animal).State = EntityState.Modified;

      try
      {
        await _db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!AnimalExists(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }
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
    

    // DELETE: api/Animals/5

    // new [HttpDelete] annotation takes an id as a URL parameter just like our equivalent GET and PUT methods. Entity doesn't care whether it gets information from an API or a web application when manipulating the database.
    // Remember that forms in HTML5 don't allow for PUT, PATCH or DELETE verbs. For that reason, we had to use HttpPost along with an ActionName like this: [HttpPost, ActionName("Delete")]. However, we aren't using HTML anymore and there are no such limitations with an API
    // For that reason, we can use HttpPut and HttpDelete.
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAnimal(int id)
    {
      var animal = await _db.Animals.FindAsync(id);
      if (animal == null)
      {
        return NotFound();
      }

      _db.Animals.Remove(animal);
      await _db.SaveChangesAsync();

      return NoContent();
    }

    // created a private method, AnimalExists, for use within the controller, to DRY up our code.
    private bool AnimalExists(int id)
    {
      return _db.Animals.Any(e => e.AnimalId == id);
    }
  }
}