using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiladores.Semantico
{
    class SemanticoException : Exception
    {
     public SemanticoException(string message) : base(message)
         {
             
        }
    }
}
