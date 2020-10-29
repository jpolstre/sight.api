using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sight.api.Models;

using sight.api;
using System;

namespace sight.api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class OrderController : ControllerBase
  {
    private readonly ApiContext _ctx;

    public OrderController(ApiContext ctx)
    {
      _ctx = ctx;
    }

    //GET api/order/pageNumber/pageSize

    [HttpGet("{pageIndex:int}/{pageSize:int}")]
    public IActionResult Get(int pageIndex, int pageSize)
    {
      var data = _ctx.Orders.Include(o => o.Customer).OrderByDescending(c => c.Placed);

      //En otras palabras  PaginationResponse<Order>, dice de que tipo sera data en este caso:  IOrderedQueryable<Order> = IEnumerable<T> en la clase
      var page = new PaginationResponse<Order>(data, pageIndex, pageSize);

      var totalCount = data.Count();
      var totalPages = Math.Ceiling((double)totalCount / pageSize);

      //asi se define un objeto.
      var response = new
      {
        Page = page,
        TotalPages = totalPages
      };

      return Ok(response);//Ok devuelve el objeto transformado en json.
    }

    //GET api/order/bystate -> agrupar por estado(Los estados de EEUU,  devuelve  {"CO":2345, "CA":567656,..eetc}).
    [HttpGet("ByState")]
    public IActionResult ByState()
    {

      var orders = _ctx.Orders.Include(o => o.Customer).ToList();

      var grupedResult = orders.GroupBy(o => o.Customer.State)
      .ToList()//guarda temporalmente la consulta en memoria.
      // la suma total de los montos de ordenes por estado(Ej. devuelve  {"CO":2345, "CA":567656,..eetc}).
      .Select(grp => new
      {
        State = grp.Key,
        Total = grp.Sum(x => x.Total)
      }).OrderByDescending(res => res.Total)
      .ToList();

      return Ok(grupedResult);
    }
    //GET api/order/bycustomer/3 -> agrupar por estado(Los estados de EEUU,  devuelve  {"ConfortTransit":2345, "PeakBakery":567656,..eetc}).
    [HttpGet("ByCustomer/{n}")]
    public IActionResult ByCustomer(int n)
    {

      var orders = _ctx.Orders.Include(o => o.Customer).ToList();

      var grupedResult = orders.GroupBy(o => o.Customer.Id)
      .ToList()//guarda temporalmente la consulta en memoria.
      // la suma total de los montos de ordenes por estado(Ej. devuelve  {"CO":2345, "CA":567656,..eetc}).
      .Select(grp => new
      {
        Name = _ctx.Customers.Find(grp.Key).Name,
        Total = grp.Sum(x => x.Total)
      }).OrderByDescending(res => res.Total)
      .Take(n)
      .ToList();

      return Ok(grupedResult);
    }

    /*GET api/order/getorder/571 =>
        {
        "id": 571,
        "customer": {
            "id": 8,
            "name": "FamilyFoods",
            "email": "familyfoods@contact.com",
            "state": "NH"
        },
        "total": 343,
        "placed": "2020-10-02T03:22:28.85805",
        "completed": "2020-10-15T03:22:28.85805"
        }
    */
    [HttpGet("GetOrder/{id}", Name = "GetOrder")]
    public IActionResult GetOrder(int id)
    {
      var order = _ctx.Orders.Include(o => o.Customer).First(o => o.Id == id);
      return Ok(order);
    }
  }
}