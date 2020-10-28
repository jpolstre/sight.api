
/*GENERAR REFISTROS PARA LAS TABLAS SEEDERS*/

using System;
using System.Collections.Generic;
using System.Linq;
using sight.api.Models;


namespace sight.api
{
  public class DataSeed
  {
    private readonly ApiContext _ctx;

    public DataSeed(ApiContext ctx)
    {

      _ctx = ctx;
    }

    public void SeedData(int nCustomers, int nOrders)
    {
      if (!_ctx.Customers.Any())
      {//si no tiene elementos

        SeedCustomers(nCustomers);
        _ctx.SaveChanges();

      }
      if (!_ctx.Orders.Any())
      {//si no tiene elementos

        SeedOrders(nOrders);
        _ctx.SaveChanges();

      }
      if (!_ctx.Servers.Any())
      {//si no tiene elementos

        SeedServers();
        _ctx.SaveChanges();

      }


    }

    private void SeedOrders(int n)
    {
      List<Order> orders = BuildOrderList(n);

      foreach (var order in orders)
      {
        _ctx.Orders.Add(order);
      }
    }

    private List<Order> BuildOrderList(int nOrders)
    {
      List<Order> orders = new List<Order>();

      var rand = new Random();

      for (var i = 1; i <= nOrders; i++)
      {
        try
        {
          var randCustomerId = rand.Next(1, _ctx.Customers.Count());
          var placed = Helpers.GetRandomOrderPlaced();
          var completed = Helpers.GetRamdomOrderCompleted(placed);
          var customers =  _ctx.Customers.ToList(); 

          orders.Add(new Order
          {
            Id = i,
            //Where devuelve una Lista de objetos(Customers) y First un único objeto
            Customer = customers.First(c => c.Id == randCustomerId),
            Total = Helpers.GetRandomOrderTotal(),
            Placed = placed,
            Completed = completed

          });
        }
        catch (Exception)
        {

        }
      }

      return orders;
    }

    private void SeedCustomers(int n)
    {
      List<Customer> customers = BuildCustomerList(n);

      foreach (var customer in customers)
      {
        //adicionar a la base dedatos( en la tabla customers)
        _ctx.Customers.Add(customer);
      }
    }


    private List<Customer> BuildCustomerList(int nCustomers)
    {
      //es como customers:Customers[] = [], en Typescript 

      var customers = new List<Customer>();
      var names = new List<string>();

      for (var i = 1; i <= nCustomers; i++)
      {

        try
        {
          var name = Helpers.MakeUniqueCustomerName(names);

          names.Add(name);

          var email = Helpers.MakeCustomerEmail(name);
          var state = Helpers.MakeCustomerState();
          customers.Add(new Customer
          {
            Id = i,
            Name = name,
            Email = email,
            State = state
          });
        }
        catch (Exception)
        {

        }
      }
      return customers;
    }



    private void SeedServers()
    {
      List<Server> servers = BuildServerList();
      foreach (var server in servers)
      {
        _ctx.Servers.Add(server);
      }

    }

    private List<Server> BuildServerList()
    {
      var servers = new List<Server>(){
        new Server{
          Id  = 1,
          Name = "Dev-Web",
          IsOnline = true
        },
        new Server{
          Id  = 2,
          Name = "Dev-Mail",
          IsOnline = false
        },
        new Server{
          Id  = 3,
          Name = "Dev-Services",
          IsOnline = true
        },
        new Server{
          Id  = 4,
          Name = "QA-Web",
          IsOnline = true
        },
        new Server{
          Id  = 5,
          Name = "QA-Mail",
          IsOnline = true
        },
        new Server{
          Id  = 6,
          Name = "QA-Services",
          IsOnline = true
        },  new Server{
          Id  = 7,
          Name = "Prod-Web",
          IsOnline = true
        },
        new Server{
          Id  = 8,
          Name = "Prod-Mail",
          IsOnline = true
        },
        new Server{
          Id  = 9,
          Name = "Prod-Services",
          IsOnline = true
        },

      };
      return servers;
    }

  }


}