
using System;
namespace sight.api.Models
{
  public class Order
  {
    public int Id { get; set; }
    /*una orden es realizada por un cliente (customer)*/
    public Customer Customer { get; set; }
    public decimal Total { get; set; }
    public DateTime Placed { get; set; }
    public DateTime? Completed { get; set; }

  }
}