using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderLibrary.Interfaces
{
    public interface IOrderService
    {
        void Buy(string code, int quantity, decimal price);
        void Sell(string code, int quantity, decimal price);
    }
}
