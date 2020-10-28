
using System;
using System.Collections.Generic;

namespace sight.api
{
  public class Helpers
  {

    //par agenerar numeros randomicos.
    private static Random _rand = new Random();
    private static readonly List<string> bizPrefix = new List<string>()
    {
        "ABC",
        "XYZ",
        "MainSt",
        "Sales",
        "Interprise",
        "Ready",
        "Quick",
        "Budget",
        "Peak",
        "Magic",
        "Family",
        "Confort"
    };
    private static readonly List<string> bizSuffix = new List<string>()
    {
        "Corporation",
        "CO",
        "Logistic",
        "Transit",
        "Bakery",
        "Goods",
        "Foods",
        "Cleaners",
        "Hotels",
        "Planners",
        "Automotive",
        "Books"
    };

    private static readonly List<string> usStates = new List<string>()
    {
      "AK", "AL", "AZ", "AR", "CA", "CO", "CT", "DE", "FL", "GA",
      "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MD",
      "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ",
      "NM", "NY", "NC", "ND", "OH", "OK", "OR", "PA", "RI", "SC",
      "SD", "TN", "TX", "UT", "VT", "VA", "WA", "WV", "WI", "WY"
    };



    //para que pueda ser utilizado sin necesidad de instanciar un objeto(por eso es internal static).

    private static string GetRandom(IList<string> items)
    {
      return items[_rand.Next(items.Count)];
    }


    internal static string MakeUniqueCustomerName(List<string> names)
    {

      //para realizar todas las combinaciones posibles y evitar que entre en un bucle infinito.
      var maxNames = bizPrefix.Count * bizSuffix.Count;
      if (names.Count >= maxNames)
      {
        throw new System.InvalidOperationException("Maximun numbers of unique names exceeded");
      }

      var prefix = GetRandom(bizPrefix);
      var suffix = GetRandom(bizSuffix);

      var bizName = prefix + suffix;//concatenar strings.

      if (!names.Contains(bizName))
      {
        return bizName;
      }
      else
      {
        //recursivo.
        return MakeUniqueCustomerName(names);

      }


    }



    internal static string MakeCustomerState()
    {
      return GetRandom(usStates);
    }

    internal static string MakeCustomerEmail(string customerName)
    {

      return $"{customerName.ToLower()}@contact.com";
    }

    static internal decimal GetRandomOrderTotal()
    {
      return _rand.Next(100, 5000);
    }

    internal static DateTime GetRandomOrderPlaced()
    {
      var end = DateTime.Now;
      var start = end.AddDays(-90);
      TimeSpan possibleSpan = end - start;
      TimeSpan newSpan = new TimeSpan(0, _rand.Next(0, (int)possibleSpan.TotalMinutes), 0);

      return start + newSpan;
    }

    internal static DateTime? GetRamdomOrderCompleted(DateTime orderPlaced)
    {
      var now = DateTime.Now;
      var minLeadTime = TimeSpan.FromDays(7);
      var timePassed = now - orderPlaced;

      if (timePassed < minLeadTime)
      {
        return null;
      }
      return orderPlaced.AddDays(_rand.Next(7, 14));

    }


  }
}