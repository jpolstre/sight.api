using System.Collections.Generic;
using System.Linq;

namespace sight.api
{
  /*T, es de tipo es cualquier tipo(cualquier clase o tipo ej: string, Customer, Order int, etc)*/
  public class PaginationResponse<T>//<T> quiere decir que en su constructor recibe cualquier tipo (T,  genérico, asi funciona para cualquier colección.)
  {
    public int Total { get; set; }

    //Data seria un enumerable del tipo instanciado en el constructor (new PaginationResponse<int>(new List<string>(){1, ....,100}, 10, 100))
    public IEnumerable<T> Data { get; set; }
    public PaginationResponse(IEnumerable<T> data, int pageIndex, int pageSize)
    {
      // a partir de 0, tomar 10 osea 0-9, a partir de 10 tomar 10 mas osea 10-19, ...
      Data = data.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
      Total = data.Count();
    }

  }
}