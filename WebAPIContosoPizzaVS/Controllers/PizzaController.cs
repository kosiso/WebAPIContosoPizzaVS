using Microsoft.AspNetCore.Mvc;
using WebAPIContosoPizzaVS.Models;
using WebAPIContosoPizzaVS.Services;
using Microsoft.Data.Sqlite;

namespace WebAPIContosoPizzaVS.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class PizzaController : ControllerBase
    {

        public PizzaController() { }

        // GET all action
        [HttpGet]
        public ActionResult<List<Pizza>> GetAll() => PizzaServiceDAL.GetAll();

        // GET by Id action
        [HttpGet("{id}")]
        public ActionResult<Pizza> Get(int id)
        {
            Pizza? pizza = PizzaServiceDAL.Get(id);
            return pizza switch
            {
                null => NotFound(),
                _ => Ok(pizza)
            };
        }

        // POST action
        [HttpPost]
        public IActionResult Create(Pizza pizza)
        {
            PizzaServiceDAL.Add(pizza);
            return CreatedAtAction(nameof(Create), new { id = pizza.Id }, pizza);
        }

        // PUT action
        [HttpPut("{id}")]
        public IActionResult Update(int id, Pizza pizza)
        {
            if (id != pizza.Id)
            {
                return BadRequest();
            }

            if (PizzaServiceDAL.Get(id) is null)
            {
                return NotFound();
            }

            PizzaServiceDAL.Update(pizza);
            return NoContent();
        }

        // DELETE action
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (PizzaServiceDAL.Get(id) is null)
            {
                return NotFound();
            }

            PizzaServiceDAL.Delete(id);
            return NoContent();
        }
    }
}
