using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderLibrary.Implemetation;

namespace OrderLibrary.Interfaces
{
    public interface IErrored
    {
        event ErroredEventHandler Errored;
    }
}
