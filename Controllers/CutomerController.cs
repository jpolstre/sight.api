using System.Linq;
using Microsoft.AspNetCore.Mvc;
using sight.api.Models;

namespace sight.api.Controllers
{

  [ApiController]
  [Route("api/[controller]")]
  public class CustomerController : ControllerBase
  {
    private readonly ApiContext _ctx;

    //constructor.
    public CustomerController(ApiContext ctx)
    {
      //_ indica propieda privada y puede ser accedida sin this.
      _ctx = ctx;
    }

    //GET api/cutomer
    [HttpGet]
    public IActionResult Get()
    {
      var data = _ctx.Customers.OrderBy(c => c.Id);

      return Ok(data);
    }

    // GET api/customer/5,  "GetCustomer" es para la ruta de poder llamar dese otras rutas.
    [HttpGet("{id}", Name = "GetCustomer")]
    public IActionResult Get(int id)
    {
      var data = _ctx.Customers.Find(id);

      return Ok(data);
    }
    [HttpPost]
    public IActionResult Post([FromBody] Customer customer)
    {
      if (customer == null)
      {
        return BadRequest();
      }

      _ctx.Customers.Add(customer);
      _ctx.SaveChanges();


      //creamos la ruta y la mandamos (retorna el nuevo customer creado)
      return CreatedAtRoute("GetCustomer", new { id = customer.Id }, customer);
    }




  }
}