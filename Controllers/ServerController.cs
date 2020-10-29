using System.Linq;
using Microsoft.AspNetCore.Mvc;
using sight.api.Models;

namespace sight.api.Controllers
{
  /*Decoradores de clase necesarios.*/
  [ApiController]
  [Route("api/[controller]")]
  public class ServerController : ControllerBase
  {
    private readonly ApiContext _ctx;

    //El controlador debe ser public, o dra error.
    public ServerController(ApiContext ctx)
    {
      _ctx = ctx;
    }

    //Get api/server
    [HttpGet]
    public IActionResult Get()
    {
      var servers = _ctx.Servers.OrderBy(s => s.Id).ToList();//es ascending por default.
      return Ok(servers);
    }
    //Get api/server/3
    [HttpGet("{id}", Name = "GetServer")]
    public IActionResult Get(int id)
    {
      var server = _ctx.Servers.Find(id);
      return Ok(server);
    }

    [HttpPut("{id}")]
    public IActionResult Message(int id, [FromBody] ServerMessage msg)
    {
      var server = _ctx.Servers.Find(id);
      if (server == null)
      {
        return NotFound();
      }

      //Refactor: move in to a service.
      server.IsOnline = msg.payload == "activate";
      //   if (msg.payload == "activate")
      //   {
      //     server.IsOnline = true;
      //   }
      //   else if (msg.payload == "deactivate")
      //   {
      //     server.IsOnline = false;

      //   }
      _ctx.SaveChanges();


      return new NoContentResult();
    }
  }
}