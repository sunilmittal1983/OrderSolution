using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderLibrary.Implemetation
{
    public class PlacedEventArgs

    {
        public string Code { get; set; }
        public decimal Price { get; set; }
        public PlacedEventArgs(string code, decimal price)

        {
            Code = code;
            Price = price;
        }
    }
}
